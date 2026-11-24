namespace Domain.Exercises;

public class Exercise
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public int CaloriesPerHour { get; private set; }
    public bool RequiresEquipment { get; private set; }
    public MuscleGroup TargetMuscleGroup { get; private set; }
    public int Sets { get; private set; }
    public int Reps { get; private set; }
    public DateTime CreatedAt { get; }

    private Exercise(
        Guid id,
        string name,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        CaloriesPerHour = caloriesPerHour;
        RequiresEquipment = requiresEquipment;
        TargetMuscleGroup = targetMuscleGroup;
        Sets = sets;
        Reps = reps;
        CreatedAt = createdAt;
    }

    public static Exercise New(
        Guid id,
        string name,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps)
    {
        return new Exercise(id, name, caloriesPerHour, requiresEquipment, targetMuscleGroup, sets, reps, DateTime.UtcNow);
    }

    public void UpdateDetails(
        string name,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps)
    {
        Name = name;
        CaloriesPerHour = caloriesPerHour;
        RequiresEquipment = requiresEquipment;
        TargetMuscleGroup = targetMuscleGroup;
        Sets = sets;
        Reps = reps;
    }
}
