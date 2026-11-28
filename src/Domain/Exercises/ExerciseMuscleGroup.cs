using Domain.MuscleGroups;

namespace Domain.Exercises;

public class ExerciseMuscleGroup
{
    public ExerciseId ExerciseId { get; private set; }
    public MuscleGroupId MuscleGroupId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;
    public MuscleGroup MuscleGroup { get; private set; } = null!;

    private ExerciseMuscleGroup()
    {
        ExerciseId = ExerciseId.Empty();
        MuscleGroupId = MuscleGroupId.Empty();
    }

    private ExerciseMuscleGroup(ExerciseId exerciseId, MuscleGroupId muscleGroupId)
    {
        ExerciseId = exerciseId;
        MuscleGroupId = muscleGroupId;
    }

    public static ExerciseMuscleGroup Create(ExerciseId exerciseId, MuscleGroupId muscleGroupId)
    {
        return new ExerciseMuscleGroup(exerciseId, muscleGroupId);
    }
}
