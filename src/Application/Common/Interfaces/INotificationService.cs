namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyExerciseCreatedAsync(
        Guid exerciseId,
        string title,
        CancellationToken cancellationToken);
}
