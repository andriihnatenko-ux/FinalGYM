using Application.Common.Interfaces;
using Application.Exercises.Services.Abstract;
using Domain.Exercises;

namespace Application.Exercises.Services.Implementation;

public class ExerciseService(IExerciseRepository exerciseRepository) : IExerciseService
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
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken)
    {
        var existingExercise = await exerciseRepository.GetByTitle(title, cancellationToken);
        if (existingExercise is not null)
        {
            throw new ArgumentException($"Exercise with title '{title}' already exists");
        }

        var exercise = Exercise.New(
            Guid.NewGuid(),
            title,
            description,
            caloriesPerHour,
            requiresEquipment,
            targetMuscleGroup,
            sets,
            reps,
            attributes);

        return await exerciseRepository.Add(exercise, cancellationToken);
    }

    public async Task<Exercise?> Update(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
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
            throw new ArgumentException($"Exercise with title '{title}' already exists");
        }

        exercise.UpdateDetails(
            title,
            description,
            caloriesPerHour,
            requiresEquipment,
            targetMuscleGroup,
            sets,
            reps,
            attributes);

        return await exerciseRepository.Update(exercise, cancellationToken);
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
}
