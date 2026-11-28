using Domain.Exercises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ExerciseMuscleGroupConfiguration : IEntityTypeConfiguration<ExerciseMuscleGroup>
{
    public void Configure(EntityTypeBuilder<ExerciseMuscleGroup> builder)
    {
        builder.ToTable("exercise_muscle_groups");

        builder.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });

        builder.Property(x => x.ExerciseId)
            .HasConversion(x => x.Value, x => new ExerciseId(x));

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.MuscleGroups)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MuscleGroup)
            .WithMany()
            .HasForeignKey(x => x.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
