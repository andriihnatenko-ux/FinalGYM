using System.ComponentModel.DataAnnotations;
using Domain.Exercises;

namespace Api.Dtos;

public record ExerciseDto(
    Guid Id,
    string Title,
    string Description,
    int CaloriesPerHour,
    bool RequiresEquipment,
    MuscleGroup TargetMuscleGroup,
    int Sets,
    int Reps,
    ExerciseAttributesDto Attributes,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static ExerciseDto FromDomainModel(Exercise exercise) =>
        new(
            exercise.Id,
            exercise.Title,
            exercise.Description,
            exercise.CaloriesPerHour,
            exercise.RequiresEquipment,
            exercise.TargetMuscleGroup,
            exercise.Sets,
            exercise.Reps,
            ExerciseAttributesDto.FromDomainModel(exercise.Attributes),
            exercise.CreatedAt,
            exercise.UpdatedAt);
}

public record ExerciseAttributesDto(
    [MaxLength(100)] string? Difficulty,
    [MaxLength(100)] string? Equipment,
    [MaxLength(500)] string? Notes)
{
    public static ExerciseAttributesDto FromDomainModel(ExerciseAttributes attributes) =>
        new(attributes.Difficulty, attributes.Equipment, attributes.Notes);

    public ExerciseAttributes ToDomainModel() =>
        ExerciseAttributes.Create(Difficulty, Equipment, Notes);
}

public record CreateExerciseDto(
    [Required, MinLength(3), MaxLength(255)] string Title,
    [Required, MinLength(3), MaxLength(500)] string Description,
    [Range(1, 5_000)] int CaloriesPerHour,
    bool RequiresEquipment,
    [EnumDataType(typeof(MuscleGroup))] MuscleGroup TargetMuscleGroup,
    [Range(1, 100)] int Sets,
    [Range(1, 100)] int Reps,
    ExerciseAttributesDto? Attributes);

public record UpdateExerciseDto(
    [Required, MinLength(3), MaxLength(255)] string Title,
    [Required, MinLength(3), MaxLength(500)] string Description,
    [Range(1, 5_000)] int CaloriesPerHour,
    bool RequiresEquipment,
    [EnumDataType(typeof(MuscleGroup))] MuscleGroup TargetMuscleGroup,
    [Range(1, 100)] int Sets,
    [Range(1, 100)] int Reps,
    ExerciseAttributesDto? Attributes);
