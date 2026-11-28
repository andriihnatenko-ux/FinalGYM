using Domain.Exercises;

namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyExerciseCreatedAsync(
        ExerciseId exerciseId,
        string title,
        CancellationToken cancellationToken);
}
