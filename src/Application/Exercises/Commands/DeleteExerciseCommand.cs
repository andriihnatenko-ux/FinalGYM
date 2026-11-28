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
        var exercise = await exerciseRepository.GetById(new ExerciseId(request.Id), cancellationToken);
        if (exercise is null)
        {
            return false;
        }

        await exerciseRepository.Delete(exercise, cancellationToken);

        return true;
    }
}
