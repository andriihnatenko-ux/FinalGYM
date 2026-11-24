namespace Domain.Exercises;

public class Exercise
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int CaloriesPerHour { get; private set; }
    public bool RequiresEquipment { get; private set; }
    public MuscleGroup TargetMuscleGroup { get; private set; }
    public int Sets { get; private set; }
    public int Reps { get; private set; }
    public ExerciseAttributes Attributes { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Exercise(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes attributes,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        CaloriesPerHour = caloriesPerHour;
        RequiresEquipment = requiresEquipment;
        TargetMuscleGroup = targetMuscleGroup;
        Sets = sets;
        Reps = reps;
        Attributes = attributes;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Exercise New(
        Guid id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes? attributes = null)
    {
        return new Exercise(
            id,
            title,
            description,
            caloriesPerHour,
            requiresEquipment,
            targetMuscleGroup,
            sets,
            reps,
            attributes ?? ExerciseAttributes.Empty(),
            DateTime.UtcNow,
            null);
    }

    public void UpdateDetails(
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        MuscleGroup targetMuscleGroup,
        int sets,
        int reps,
        ExerciseAttributes? attributes = null)
    {
        Title = title;
        Description = description;
        CaloriesPerHour = caloriesPerHour;
        RequiresEquipment = requiresEquipment;
        TargetMuscleGroup = targetMuscleGroup;
        Sets = sets;
        Reps = reps;

        if (attributes is not null)
        {
            Attributes.Update(attributes.Difficulty, attributes.Equipment, attributes.Notes);
        }

        UpdatedAt = DateTime.UtcNow;
    }
}
