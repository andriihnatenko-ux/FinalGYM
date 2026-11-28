namespace Domain.MuscleGroups;

public record MuscleGroupId(Guid Value)
{
    public static MuscleGroupId Empty() => new(Guid.Empty);

    public static MuscleGroupId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
