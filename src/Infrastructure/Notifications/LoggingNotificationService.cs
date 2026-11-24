using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Notifications;

public class LoggingNotificationService(ILogger<LoggingNotificationService> logger) : INotificationService
{
    public Task NotifyExerciseCreatedAsync(
        Guid exerciseId,
        string title,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Exercise {ExerciseId} ({Title}) was created",
            exerciseId,
            title);

        return Task.CompletedTask;
    }
}
