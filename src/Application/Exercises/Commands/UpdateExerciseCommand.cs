using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using Domain.MuscleGroups;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public record UpdateExerciseCommand : IRequest<Either<ExerciseException, Exercise>>
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
    : IRequestHandler<UpdateExerciseCommand, Either<ExerciseException, Exercise>>
{
    public async Task<Either<ExerciseException, Exercise>> Handle(
        UpdateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseId = new ExerciseId(request.Id);
        var exerciseOption = await exerciseRepository.GetById(exerciseId, cancellationToken);

        return await exerciseOption.Match<Task<Either<ExerciseException, Exercise>>>(
            Some: exercise => UpdateEntity(exercise, request, cancellationToken),
            None: () => Task.FromResult<Either<ExerciseException, Exercise>>(
                new ExerciseNotFoundException(exerciseId)));
    }

    private async Task<Either<ExerciseException, Exercise>> UpdateEntity(
        Exercise exercise,
        UpdateExerciseCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseWithSameTitle = await exerciseRepository.GetByTitle(
            request.Title,
            cancellationToken);

        if (exerciseWithSameTitle.Match(x => x.Id != exercise.Id, () => false))
        {
            return new ExerciseAlreadyExistsException(exercise.Id, request.Title);
        }

        var missingGroupIds = await ExerciseMuscleGroupGuard.FindMissing(
            muscleGroupRepository,
            request.MuscleGroupIds,
            cancellationToken);

        if (missingGroupIds.Count > 0)
        {
            return new ExerciseGroupsNotFoundException(exercise.Id, missingGroupIds);
        }

        try
        {
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

            var reloadedExercise = await exerciseRepository.GetById(exercise.Id, cancellationToken);
            return reloadedExercise.Match(x => x, () => exercise);
        }
        catch (Exception exception)
        {
            return new ExerciseUnhandledException(exercise.Id, exception);
        }
    }
}
