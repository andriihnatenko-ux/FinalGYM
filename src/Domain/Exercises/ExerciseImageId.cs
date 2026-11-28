namespace Domain.Exercises;

public record ExerciseImageId(Guid Value)
{
    public static ExerciseImageId Empty() => new(Guid.Empty);

    public static ExerciseImageId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
