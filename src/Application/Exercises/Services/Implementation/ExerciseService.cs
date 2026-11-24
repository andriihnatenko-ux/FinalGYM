using Application.Common.Interfaces;
using Application.Exercises.Exceptions;
using Application.Exercises.Services.Abstract;
using Domain.Exercises;

namespace Application.Exercises.Services.Implementation;

public class ExerciseService(
    IExerciseRepository exerciseRepository,
    IMuscleGroupRepository muscleGroupRepository,
    INotificationService notificationService,
    IFileStorage fileStorage) : IExerciseService
{
    public async Task<IReadOnlyList<Exercise>> GetExercises(CancellationToken cancellationToken)
    {
        return await exerciseRepository.GetAll(cancellationToken);
    }

    public async Task<Exercise?> GetExercise(Guid id, CancellationToken cancellationToken)
    {
        return await exerciseRepository.GetById(id, cancellationToken);
    }

    public async Task<Exercise> Add(
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        int sets,
        int reps,
        IReadOnlyList<Guid> muscleGroupIds,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken)
    {
        var existingExercise = await exerciseRepository.GetByTitle(title, cancellationToken);
        if (existingExercise is not null)
        {
            throw new ExerciseAlreadyExistsException(title);
        }

        await EnsureMuscleGroupsExist(muscleGroupIds, cancellationToken);

        var exercise = Exercise.New(
            Guid.NewGuid(),
            title,
            description,
            caloriesPerHour,
            requiresEquipment,
            sets,
            reps,
            attributes);

        exercise.SyncMuscleGroups(muscleGroupIds);

        await exerciseRepository.Add(exercise, cancellationToken);
        await notificationService.NotifyExerciseCreatedAsync(exercise.Id, exercise.Title, cancellationToken);

        return await exerciseRepository.GetById(exercise.Id, cancellationToken) ?? exercise;
    }

    public async Task<Exercise?> Update(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        int sets,
        int reps,
        IReadOnlyList<Guid> muscleGroupIds,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetById(id, cancellationToken);
        if (exercise is null)
        {
            return null;
        }

        var exerciseWithSameTitle = await exerciseRepository.GetByTitle(title, cancellationToken);
        if (exerciseWithSameTitle is not null && exerciseWithSameTitle.Id != id)
        {
            throw new ExerciseAlreadyExistsException(title);
        }

        await EnsureMuscleGroupsExist(muscleGroupIds, cancellationToken);

        exercise.UpdateDetails(
            title,
            description,
            caloriesPerHour,
            requiresEquipment,
            sets,
            reps,
            attributes);

        exercise.SyncMuscleGroups(muscleGroupIds);

        await exerciseRepository.Update(exercise, cancellationToken);

        return await exerciseRepository.GetById(id, cancellationToken) ?? exercise;
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetById(id, cancellationToken);
        if (exercise is null)
        {
            return false;
        }

        await exerciseRepository.Delete(exercise, cancellationToken);

        return true;
    }

    public async Task<ExerciseImage?> AddImage(
        Guid exerciseId,
        string originalName,
        Stream content,
        CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetById(exerciseId, cancellationToken);
        if (exercise is null)
        {
            return null;
        }

        var image = ExerciseImage.New(Guid.NewGuid(), originalName, exerciseId);
        await exerciseRepository.AddImage(image, cancellationToken);

        var extension = Path.GetExtension(originalName);
        await fileStorage.SaveAsync(exerciseId, image.Id, extension, content, cancellationToken);

        return image;
    }

    private async Task EnsureMuscleGroupsExist(
        IReadOnlyList<Guid> muscleGroupIds,
        CancellationToken cancellationToken)
    {
        if (muscleGroupIds.Count == 0)
        {
            throw new InvalidExerciseRequestException("At least one muscle group is required.");
        }

        if (muscleGroupIds.Any(id => id == Guid.Empty))
        {
            throw new InvalidExerciseRequestException("Muscle group ids must not be empty.");
        }

        var distinctIds = muscleGroupIds.Distinct().ToList();
        var existingGroups = await muscleGroupRepository.GetByIds(distinctIds, cancellationToken);
        var existingIds = existingGroups.Select(x => x.Id).ToHashSet();
        var missingIds = distinctIds.Where(id => !existingIds.Contains(id)).ToList();

        if (missingIds.Count > 0)
        {
            throw new MuscleGroupsNotFoundException(missingIds);
        }
    }
}
