namespace Domain.Exercises;

public record ExerciseId(Guid Value)
{
    public static ExerciseId Empty() => new(Guid.Empty);

    public static ExerciseId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
