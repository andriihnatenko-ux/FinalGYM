using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using Domain.MuscleGroups;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public record CreateExerciseCommand : IRequest<Either<ExerciseException, Exercise>>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int CaloriesPerHour { get; init; }
    public required bool RequiresEquipment { get; init; }
    public required int Sets { get; init; }
    public required int Reps { get; init; }
    public required IReadOnlyList<Guid> MuscleGroupIds { get; init; }
    public ExerciseAttributes? Attributes { get; init; }
}

public class CreateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IMuscleGroupRepository muscleGroupRepository,
    INotificationService notificationService)
    : IRequestHandler<CreateExerciseCommand, Either<ExerciseException, Exercise>>
{
    public async Task<Either<ExerciseException, Exercise>> Handle(
        CreateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var existingExercise = await exerciseRepository.GetByTitle(request.Title, cancellationToken);

        return await existingExercise.Match<Task<Either<ExerciseException, Exercise>>>(
            Some: _ => Task.FromResult<Either<ExerciseException, Exercise>>(
                new ExerciseAlreadyExistsException(ExerciseId.Empty(), request.Title)),
            None: () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<ExerciseException, Exercise>> CreateEntity(
        CreateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var missingGroupIds = await ExerciseMuscleGroupGuard.FindMissing(
            muscleGroupRepository,
            request.MuscleGroupIds,
            cancellationToken);

        if (missingGroupIds.Count > 0)
        {
            return new ExerciseGroupsNotFoundException(ExerciseId.Empty(), missingGroupIds);
        }

        try
        {
            var exercise = Exercise.New(
                ExerciseId.New(),
                request.Title,
                request.Description,
                request.CaloriesPerHour,
                request.RequiresEquipment,
                request.Sets,
                request.Reps,
                request.Attributes);

            exercise.SyncMuscleGroups(
                request.MuscleGroupIds.Select(id => new MuscleGroupId(id)));

            await exerciseRepository.Add(exercise, cancellationToken);
            await notificationService.NotifyExerciseCreatedAsync(
                exercise.Id,
                exercise.Title,
                cancellationToken);

            var reloadedExercise = await exerciseRepository.GetById(exercise.Id, cancellationToken);
            return reloadedExercise.Match(x => x, () => exercise);
        }
        catch (Exception exception)
        {
            return new ExerciseUnhandledException(ExerciseId.Empty(), exception);
        }
    }
}
