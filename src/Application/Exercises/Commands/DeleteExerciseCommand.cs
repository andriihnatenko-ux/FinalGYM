using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Exercises.Commands;

public record DeleteExerciseCommand : IRequest<Either<ExerciseException, Unit>>
{
    public required Guid Id { get; init; }
}

public class DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository)
    : IRequestHandler<DeleteExerciseCommand, Either<ExerciseException, Unit>>
{
    public async Task<Either<ExerciseException, Unit>> Handle(
        DeleteExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseId = new ExerciseId(request.Id);
        var exerciseOption = await exerciseRepository.GetById(exerciseId, cancellationToken);

        return await exerciseOption.Match<Task<Either<ExerciseException, Unit>>>(
            Some: exercise => DeleteEntity(exercise, cancellationToken),
            None: () => Task.FromResult<Either<ExerciseException, Unit>>(
                new ExerciseNotFoundException(exerciseId)));
    }

    private async Task<Either<ExerciseException, Unit>> DeleteEntity(
        Exercise exercise,
        CancellationToken cancellationToken)
    {
        try
        {
            await exerciseRepository.Delete(exercise, cancellationToken);
            return Unit.Default;
        }
        catch (Exception exception)
        {
            return new ExerciseUnhandledException(exercise.Id, exception);
        }
    }
}
