using Domain.Exercises;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.HasIndex(x => x.Title)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(x => x.CaloriesPerHour).IsRequired();
        builder.Property(x => x.RequiresEquipment).IsRequired();
        builder.Property(x => x.Sets).IsRequired();
        builder.Property(x => x.Reps).IsRequired();

        builder.Property(x => x.TargetMuscleGroup)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.OwnsOne(x => x.Attributes, attributes =>
        {
            attributes.ToJson();
            attributes.Property(x => x.Difficulty).HasMaxLength(100);
            attributes.Property(x => x.Equipment).HasMaxLength(100);
            attributes.Property(x => x.Notes).HasMaxLength(500);
        });

        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new NullableDateTimeUtcConverter())
            .HasColumnType("timestamp with time zone");
    }
}
