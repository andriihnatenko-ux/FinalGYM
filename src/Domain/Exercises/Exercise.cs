using Domain.MuscleGroups;

namespace Domain.Exercises;

public class Exercise
{
    private readonly List<ExerciseMuscleGroup> _muscleGroups = [];
    private readonly List<ExerciseImage> _images = [];

    public ExerciseId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int CaloriesPerHour { get; private set; }
    public bool RequiresEquipment { get; private set; }
    public int Sets { get; private set; }
    public int Reps { get; private set; }
    public ExerciseAttributes Attributes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<ExerciseMuscleGroup> MuscleGroups => _muscleGroups;
    public IReadOnlyCollection<ExerciseImage> Images => _images;

    private Exercise()
    {
        Id = ExerciseId.Empty();
        Title = string.Empty;
        Description = string.Empty;
        Attributes = ExerciseAttributes.Empty();
    }

    public static Exercise New(
        ExerciseId id,
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        int sets,
        int reps,
        ExerciseAttributes? attributes = null)
    {
        return new Exercise
        {
            Id = id,
            Title = title,
            Description = description,
            CaloriesPerHour = caloriesPerHour,
            RequiresEquipment = requiresEquipment,
            Sets = sets,
            Reps = reps,
            Attributes = attributes ?? ExerciseAttributes.Empty(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(
        string title,
        string description,
        int caloriesPerHour,
        bool requiresEquipment,
        int sets,
        int reps,
        ExerciseAttributes? attributes = null)
    {
        Title = title;
        Description = description;
        CaloriesPerHour = caloriesPerHour;
        RequiresEquipment = requiresEquipment;
        Sets = sets;
        Reps = reps;

        if (attributes is not null)
        {
            Attributes.Update(attributes.Difficulty, attributes.Equipment, attributes.Notes);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void SyncMuscleGroups(IEnumerable<MuscleGroupId> muscleGroupIds)
    {
        var targetIds = muscleGroupIds.Distinct().ToHashSet();

        _muscleGroups.RemoveAll(link => !targetIds.Contains(link.MuscleGroupId));

        var existingIds = _muscleGroups.Select(link => link.MuscleGroupId).ToHashSet();
        foreach (var muscleGroupId in targetIds.Where(id => !existingIds.Contains(id)))
        {
            _muscleGroups.Add(ExerciseMuscleGroup.Create(Id, muscleGroupId));
        }
    }

    public void AddImage(ExerciseImage image)
    {
        _images.Add(image);
    }
}
