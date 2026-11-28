using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
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
        var exercise = await exerciseRepository.GetById(request.Id, cancellationToken);
        if (exercise is null)
        {
            return null;
        }

        var exerciseWithSameTitle = await exerciseRepository.GetByTitle(request.Title, cancellationToken);
        if (exerciseWithSameTitle is not null && exerciseWithSameTitle.Id != request.Id)
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

        exercise.SyncMuscleGroups(request.MuscleGroupIds);

        await exerciseRepository.Update(exercise, cancellationToken);

        return await exerciseRepository.GetById(request.Id, cancellationToken) ?? exercise;
    }
}
