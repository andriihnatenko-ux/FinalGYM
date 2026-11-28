using Domain.Exercises;
using Domain.MuscleGroups;

namespace Application.Exercises.Exceptions;

public abstract class ExerciseException(
    ExerciseId exerciseId,
    string message,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public ExerciseId ExerciseId { get; } = exerciseId;
}

public class ExerciseAlreadyExistsException(ExerciseId exerciseId, string title)
    : ExerciseException(exerciseId, $"Exercise with title '{title}' already exists");

public class ExerciseNotFoundException(ExerciseId exerciseId)
    : ExerciseException(exerciseId, $"Exercise not found under id {exerciseId}");

public class ExerciseGroupsNotFoundException(
    ExerciseId exerciseId,
    IReadOnlyList<MuscleGroupId> missingGroupIds)
    : ExerciseException(
        exerciseId,
        $"Muscle groups not found: {string.Join(", ", missingGroupIds)}")
{
    public IReadOnlyList<MuscleGroupId> MissingGroupIds { get; } = missingGroupIds;
}

public class ExerciseUnhandledException(ExerciseId exerciseId, Exception? innerException = null)
    : ExerciseException(
        exerciseId,
        $"Unhandled exception under exercise {exerciseId}",
        innerException);
