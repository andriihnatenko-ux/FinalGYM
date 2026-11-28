using Application.Common.Interfaces.Repositories;
using Domain.Exercises;
using MediatR;

namespace Application.Exercises.Commands;

public record DeleteExerciseCommand : IRequest<bool>
{
    public required Guid Id { get; init; }
}

public class DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<DeleteExerciseCommand, bool>
{
    public async Task<bool> Handle(
        DeleteExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseOption = await exerciseRepository.GetById(new ExerciseId(request.Id), cancellationToken);
        if (exerciseOption.IsNone)
        {
            return false;
        }

        var exercise = exerciseOption.Match(x => x, () => null!);
        await exerciseRepository.Delete(exercise, cancellationToken);

        return true;
    }
}
