namespace Domain.Exercises;

public class ExerciseImage
{
    public Guid Id { get; }
    public string OriginalName { get; private set; }
    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;

    private ExerciseImage()
    {
        OriginalName = string.Empty;
    }

    private ExerciseImage(Guid id, string originalName, Guid exerciseId)
    {
        Id = id;
        OriginalName = originalName;
        ExerciseId = exerciseId;
    }

    public static ExerciseImage New(Guid id, string originalName, Guid exerciseId)
    {
        return new ExerciseImage(id, originalName, exerciseId);
    }
}
