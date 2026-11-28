using Domain.MuscleGroups;

namespace Domain.Exercises;

public class ExerciseMuscleGroup
{
    public ExerciseId ExerciseId { get; private set; }
    public Guid MuscleGroupId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;
    public MuscleGroup MuscleGroup { get; private set; } = null!;

    private ExerciseMuscleGroup()
    {
        ExerciseId = ExerciseId.Empty();
    }

    private ExerciseMuscleGroup(ExerciseId exerciseId, Guid muscleGroupId)
    {
        ExerciseId = exerciseId;
        MuscleGroupId = muscleGroupId;
    }

    public static ExerciseMuscleGroup Create(ExerciseId exerciseId, Guid muscleGroupId)
    {
        return new ExerciseMuscleGroup(exerciseId, muscleGroupId);
    }
}
