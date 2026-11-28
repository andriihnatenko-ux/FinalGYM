using Domain.MuscleGroups;

namespace Application.Common.Interfaces.Repositories;

public interface IMuscleGroupRepository
{
    Task<MuscleGroup?> GetById(MuscleGroupId id, CancellationToken cancellationToken);

    Task<MuscleGroup?> GetByName(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<MuscleGroupId> ids,
        CancellationToken cancellationToken);

    Task<MuscleGroup> Add(MuscleGroup muscleGroup, CancellationToken cancellationToken);
}
