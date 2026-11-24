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
        int sets,
        int reps,
        IReadOnlyList<Guid> muscleGroupIds,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken);

    Task<Exercise?> Update(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        int sets,
        int reps,
        IReadOnlyList<Guid> muscleGroupIds,
        ExerciseAttributes? attributes,
        CancellationToken cancellationToken);

    Task<bool> Delete(Guid id, CancellationToken cancellationToken);

    Task<ExerciseImage?> AddImage(
        Guid exerciseId,
        string originalName,
        Stream content,
        CancellationToken cancellationToken);
}
