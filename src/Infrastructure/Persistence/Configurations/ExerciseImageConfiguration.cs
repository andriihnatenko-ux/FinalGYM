using Domain.Exercises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ExerciseImageConfiguration : IEntityTypeConfiguration<ExerciseImage>
{
    public void Configure(EntityTypeBuilder<ExerciseImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OriginalName)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
