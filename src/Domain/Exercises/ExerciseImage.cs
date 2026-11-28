namespace Domain.Exercises;

public class ExerciseImage
{
    public Guid Id { get; }
    public string OriginalName { get; private set; }
    public ExerciseId ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;

    private ExerciseImage()
    {
        ExerciseId = ExerciseId.Empty();
        OriginalName = string.Empty;
    }

    private ExerciseImage(Guid id, string originalName, ExerciseId exerciseId)
    {
        Id = id;
        OriginalName = originalName;
        ExerciseId = exerciseId;
    }

    public static ExerciseImage New(Guid id, string originalName, ExerciseId exerciseId)
    {
        return new ExerciseImage(id, originalName, exerciseId);
    }
}
