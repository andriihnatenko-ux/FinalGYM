using Application.Common.Interfaces;
using Infrastructure.Notifications;
using Infrastructure.Persistence;
using Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);

        services.AddSingleton<IFileStorage, LocalFileStorage>();
        services.AddScoped<INotificationService, LoggingNotificationService>();
    }
}
