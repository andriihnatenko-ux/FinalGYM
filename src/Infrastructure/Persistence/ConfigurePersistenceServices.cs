using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(warnings =>
            {
                warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning);
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
            }));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ExerciseRepository>();
        services.AddScoped<IExerciseRepository>(provider => provider.GetRequiredService<ExerciseRepository>());
        services.AddScoped<IExerciseQueries>(provider => provider.GetRequiredService<ExerciseRepository>());
        services.AddScoped<MuscleGroupRepository>();
        services.AddScoped<IMuscleGroupRepository>(provider => provider.GetRequiredService<MuscleGroupRepository>());
        services.AddScoped<IMuscleGroupQueries>(provider => provider.GetRequiredService<MuscleGroupRepository>());
    }
}
