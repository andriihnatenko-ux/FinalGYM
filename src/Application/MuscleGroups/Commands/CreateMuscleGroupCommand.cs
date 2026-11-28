using Application.Common.Interfaces.Repositories;
using Application.MuscleGroups.Exceptions;
using Domain.MuscleGroups;
using MediatR;

namespace Application.MuscleGroups.Commands;

public record CreateMuscleGroupCommand : IRequest<MuscleGroup>
{
    public required string Name { get; init; }
}

public class CreateMuscleGroupCommandHandler(IMuscleGroupRepository muscleGroupRepository)
    : IRequestHandler<CreateMuscleGroupCommand, MuscleGroup>
{
    public async Task<MuscleGroup> Handle(
        CreateMuscleGroupCommand request,
        CancellationToken cancellationToken)
    {
        var existingMuscleGroup = await muscleGroupRepository.GetByName(request.Name, cancellationToken);
        if (existingMuscleGroup.IsSome)
        {
            throw new MuscleGroupAlreadyExistsException(request.Name);
        }

        var muscleGroup = MuscleGroup.New(MuscleGroupId.New(), request.Name);

        return await muscleGroupRepository.Add(muscleGroup, cancellationToken);
    }
}
