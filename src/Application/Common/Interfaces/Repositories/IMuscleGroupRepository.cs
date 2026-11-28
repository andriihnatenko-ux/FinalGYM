using Domain.MuscleGroups;

namespace Application.Common.Interfaces.Repositories;

public interface IMuscleGroupRepository
{
    Task<MuscleGroup?> GetById(Guid id, CancellationToken cancellationToken);

    Task<MuscleGroup?> GetByName(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);

    Task<MuscleGroup> Add(MuscleGroup muscleGroup, CancellationToken cancellationToken);
}
