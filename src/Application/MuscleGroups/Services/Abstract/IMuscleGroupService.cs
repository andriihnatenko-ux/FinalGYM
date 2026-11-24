using Domain.MuscleGroups;

namespace Application.MuscleGroups.Services.Abstract;

public interface IMuscleGroupService
{
    Task<IReadOnlyList<MuscleGroup>> GetMuscleGroups(CancellationToken cancellationToken);

    Task<MuscleGroup?> GetMuscleGroup(Guid id, CancellationToken cancellationToken);

    Task<MuscleGroup> Add(string name, CancellationToken cancellationToken);
}
