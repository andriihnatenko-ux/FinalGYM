using System.ComponentModel.DataAnnotations;
using Domain.Exercises;

namespace Api.Dtos;

public record ExerciseDto(
    Guid Id,
    string Name,
    int CaloriesPerHour,
    bool RequiresEquipment,
    MuscleGroup TargetMuscleGroup,
    int Sets,
    int Reps,
    DateTime CreatedAt)
{
    public static ExerciseDto FromDomainModel(Exercise exercise) =>
        new(
            exercise.Id,
            exercise.Name,
            exercise.CaloriesPerHour,
            exercise.RequiresEquipment,
            exercise.TargetMuscleGroup,
            exercise.Sets,
            exercise.Reps,
            exercise.CreatedAt);
}

public record CreateExerciseDto(
    [Required, MaxLength(100)] string Name,
    [Range(1, 5_000)] int CaloriesPerHour,
    bool RequiresEquipment,
    [EnumDataType(typeof(MuscleGroup))] MuscleGroup TargetMuscleGroup,
    [Range(1, 100)] int Sets,
    [Range(1, 100)] int Reps);

public record UpdateExerciseDto(
    [Required, MaxLength(100)] string Name,
    [Range(1, 5_000)] int CaloriesPerHour,
    bool RequiresEquipment,
    [EnumDataType(typeof(MuscleGroup))] MuscleGroup TargetMuscleGroup,
    [Range(1, 100)] int Sets,
    [Range(1, 100)] int Reps);
