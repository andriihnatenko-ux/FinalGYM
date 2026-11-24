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
        string name,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        CancellationToken cancellationToken)
    {
        var existingExercise = await exerciseRepository.GetByName(name, cancellationToken);
        if (existingExercise is not null)
        {
            throw new ArgumentException($"Exercise with name '{name}' already exists");
        }

        var exercise = Exercise.New(
            Guid.NewGuid(),
            name,
            caloriesPerHour,
            requiresEquipment,
            targetMuscleGroup,
            sets,
            reps);

        return await exerciseRepository.Add(exercise, cancellationToken);
    }

    public async Task<Exercise?> Update(
        Guid id,
        string name,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetById(id, cancellationToken);
        if (exercise is null)
        {
            return null;
        }

        var exerciseWithSameName = await exerciseRepository.GetByName(name, cancellationToken);
        if (exerciseWithSameName is not null && exerciseWithSameName.Id != id)
        {
            throw new ArgumentException($"Exercise with name '{name}' already exists");
        }

        exercise.UpdateDetails(name, caloriesPerHour, requiresEquipment, targetMuscleGroup, sets, reps);

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
