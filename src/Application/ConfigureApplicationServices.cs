using Application.Exercises.Services.Abstract;
using Application.Exercises.Services.Implementation;
using Application.MuscleGroups.Services.Abstract;
using Application.MuscleGroups.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IMuscleGroupService, MuscleGroupService>();
    }
}
