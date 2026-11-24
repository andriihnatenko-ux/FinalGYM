using Domain.MuscleGroups;

namespace Domain.Exercises;

public class ExerciseMuscleGroup
{
    public Guid ExerciseId { get; private set; }
    public Guid MuscleGroupId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;
    public MuscleGroup MuscleGroup { get; private set; } = null!;

    private ExerciseMuscleGroup()
    {
    }

    private ExerciseMuscleGroup(Guid exerciseId, Guid muscleGroupId)
    {
        ExerciseId = exerciseId;
        MuscleGroupId = muscleGroupId;
    }

    public static ExerciseMuscleGroup Create(Guid exerciseId, Guid muscleGroupId)
    {
        return new ExerciseMuscleGroup(exerciseId, muscleGroupId);
    }
}
