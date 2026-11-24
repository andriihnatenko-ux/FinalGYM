namespace Domain.MuscleGroups;

public class MuscleGroup
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; }

    private MuscleGroup(Guid id, string name, DateTime createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
    }

    public static MuscleGroup New(Guid id, string name)
    {
        return new MuscleGroup(id, name, DateTime.UtcNow);
    }

    public void UpdateDetails(string name)
    {
        Name = name;
    }
}
