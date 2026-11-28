using Application.Common.Interfaces.Repositories;
using Application.MuscleGroups.Exceptions;
using Domain.MuscleGroups;
using LanguageExt;
using MediatR;

namespace Application.MuscleGroups.Commands;

public record CreateMuscleGroupCommand : IRequest<Either<MuscleGroupException, MuscleGroup>>
{
    public required string Name { get; init; }
}

public class CreateMuscleGroupCommandHandler(IMuscleGroupRepository muscleGroupRepository)
    : IRequestHandler<CreateMuscleGroupCommand, Either<MuscleGroupException, MuscleGroup>>
{
    public async Task<Either<MuscleGroupException, MuscleGroup>> Handle(
        CreateMuscleGroupCommand request,
        CancellationToken cancellationToken)
    {
        var existingMuscleGroup = await muscleGroupRepository.GetByName(
            request.Name,
            cancellationToken);

        return await existingMuscleGroup.Match<Task<Either<MuscleGroupException, MuscleGroup>>>(
            Some: _ => Task.FromResult<Either<MuscleGroupException, MuscleGroup>>(
                new MuscleGroupAlreadyExistsException(MuscleGroupId.Empty(), request.Name)),
            None: () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<MuscleGroupException, MuscleGroup>> CreateEntity(
        CreateMuscleGroupCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var muscleGroup = MuscleGroup.New(MuscleGroupId.New(), request.Name);
            return await muscleGroupRepository.Add(muscleGroup, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MuscleGroupUnhandledException(MuscleGroupId.Empty(), exception);
        }
    }
}
