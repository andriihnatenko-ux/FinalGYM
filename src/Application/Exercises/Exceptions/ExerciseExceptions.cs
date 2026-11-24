namespace Application.Exercises.Exceptions;

public class ExerciseAlreadyExistsException(string title)
    : Exception($"Exercise with title '{title}' already exists");

public class MuscleGroupsNotFoundException(IReadOnlyList<Guid> missingGroupIds)
    : Exception($"Muscle groups not found: {string.Join(", ", missingGroupIds)}")
{
    public IReadOnlyList<Guid> MissingGroupIds { get; } = missingGroupIds;
}

public class InvalidExerciseRequestException(string message) : Exception(message);
