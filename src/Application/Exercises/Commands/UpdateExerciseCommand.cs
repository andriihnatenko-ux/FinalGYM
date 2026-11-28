using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using Domain.MuscleGroups;
using MediatR;

namespace Application.Exercises.Commands;

public record UpdateExerciseCommand : IRequest<Exercise?>
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int CaloriesPerHour { get; init; }
    public required bool RequiresEquipment { get; init; }
    public required int Sets { get; init; }
    public required int Reps { get; init; }
    public required IReadOnlyList<Guid> MuscleGroupIds { get; init; }
    public ExerciseAttributes? Attributes { get; init; }
}

public class UpdateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IMuscleGroupRepository muscleGroupRepository)
    : IRequestHandler<UpdateExerciseCommand, Exercise?>
{
    public async Task<Exercise?> Handle(
        UpdateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseId = new ExerciseId(request.Id);
        var exerciseOption = await exerciseRepository.GetById(exerciseId, cancellationToken);
        if (exerciseOption.IsNone)
        {
            return null;
        }

        var exercise = exerciseOption.Match(x => x, () => null!);

        var exerciseWithSameTitle = await exerciseRepository.GetByTitle(request.Title, cancellationToken);
        if (exerciseWithSameTitle.Match(x => x.Id != exerciseId, () => false))
        {
            throw new ExerciseAlreadyExistsException(request.Title);
        }

        await ExerciseMuscleGroupGuard.EnsureExist(
            muscleGroupRepository,
            request.MuscleGroupIds,
            cancellationToken);

        exercise.UpdateDetails(
            request.Title,
            request.Description,
            request.CaloriesPerHour,
            request.RequiresEquipment,
            request.Sets,
            request.Reps,
            request.Attributes);

        exercise.SyncMuscleGroups(
            request.MuscleGroupIds.Select(id => new MuscleGroupId(id)));

        await exerciseRepository.Update(exercise, cancellationToken);

        var reloadedExercise = await exerciseRepository.GetById(exerciseId, cancellationToken);
        return reloadedExercise.Match<Exercise?>(x => x, () => exercise);
    }
}
