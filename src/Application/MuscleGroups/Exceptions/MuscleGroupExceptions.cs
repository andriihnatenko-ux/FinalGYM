namespace Application.MuscleGroups.Exceptions;

public class MuscleGroupAlreadyExistsException(string name)
    : Exception($"Muscle group with name '{name}' already exists");
