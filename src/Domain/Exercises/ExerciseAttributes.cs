namespace Domain.Exercises;

public class ExerciseAttributes
{
    public string? Difficulty { get; private set; }
    public string? Equipment { get; private set; }
    public string? Notes { get; private set; }

    private ExerciseAttributes()
    {
    }

    public static ExerciseAttributes Empty() => new();

    public static ExerciseAttributes Create(string? difficulty, string? equipment, string? notes)
    {
        return new ExerciseAttributes
        {
            Difficulty = difficulty,
            Equipment = equipment,
            Notes = notes
        };
    }

    public void Update(string? difficulty, string? equipment, string? notes)
    {
        Difficulty = difficulty;
        Equipment = equipment;
        Notes = notes;
    }
}
