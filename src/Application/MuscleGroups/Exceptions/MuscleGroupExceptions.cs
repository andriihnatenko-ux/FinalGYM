using Domain.MuscleGroups;

namespace Application.MuscleGroups.Exceptions;

public abstract class MuscleGroupException(
    MuscleGroupId muscleGroupId,
    string message,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public MuscleGroupId MuscleGroupId { get; } = muscleGroupId;
}

public class MuscleGroupAlreadyExistsException(MuscleGroupId muscleGroupId, string name)
    : MuscleGroupException(muscleGroupId, $"Muscle group with name '{name}' already exists");

public class MuscleGroupNotFoundException(MuscleGroupId muscleGroupId)
    : MuscleGroupException(muscleGroupId, $"Muscle group not found under id {muscleGroupId}");

public class MuscleGroupUnhandledException(
    MuscleGroupId muscleGroupId,
    Exception? innerException = null)
    : MuscleGroupException(
        muscleGroupId,
        $"Unhandled exception under muscle group {muscleGroupId}",
        innerException);
