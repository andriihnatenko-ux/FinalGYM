using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using Domain.MuscleGroups;
using MediatR;

namespace Application.Exercises.Commands;

public record CreateExerciseCommand : IRequest<Exercise>
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
    : IRequestHandler<CreateExerciseCommand, Exercise>
{
    public async Task<Exercise> Handle(
        CreateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var existingExercise = await exerciseRepository.GetByTitle(request.Title, cancellationToken);
        if (existingExercise.IsSome)
        {
            throw new ExerciseAlreadyExistsException(request.Title);
        }

        await ExerciseMuscleGroupGuard.EnsureExist(
            muscleGroupRepository,
            request.MuscleGroupIds,
            cancellationToken);

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
}
