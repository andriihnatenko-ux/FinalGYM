using System.Reflection;
using Domain.Exercises;
using Microsoft.EntityFrameworkCore;
using MuscleGroupEntity = Domain.MuscleGroups.MuscleGroup;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Exercise> Exercises { get; init; }

    public DbSet<MuscleGroupEntity> MuscleGroups { get; init; }

    public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups { get; init; }

    public DbSet<ExerciseImage> ExerciseImages { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
