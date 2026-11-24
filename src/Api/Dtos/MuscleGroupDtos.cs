using System.ComponentModel.DataAnnotations;
using Domain.MuscleGroups;

namespace Api.Dtos;

public record MuscleGroupDto(Guid Id, string Name, DateTime CreatedAt)
{
    public static MuscleGroupDto FromDomainModel(MuscleGroup muscleGroup) =>
        new(muscleGroup.Id, muscleGroup.Name, muscleGroup.CreatedAt);
}

public record CreateMuscleGroupDto(
    [Required, MinLength(3), MaxLength(255)] string Name);
