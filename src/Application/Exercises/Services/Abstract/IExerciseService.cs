using Domain.Exercises;

namespace Application.Exercises.Services.Abstract;

public interface IExerciseService
{
    Task<IReadOnlyList<Exercise>> GetExercises(CancellationToken cancellationToken);

    Task<Exercise?> GetExercise(Guid id, CancellationToken cancellationToken);

    Task<Exercise> Add(
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken);

    Task<Exercise?> Update(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken);

    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}
