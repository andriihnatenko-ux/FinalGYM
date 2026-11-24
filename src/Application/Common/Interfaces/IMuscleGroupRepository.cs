using Domain.MuscleGroups;

namespace Application.Common.Interfaces;

public interface IMuscleGroupRepository
{
    Task<IReadOnlyList<MuscleGroup>> GetAll(CancellationToken cancellationToken);

    Task<MuscleGroup?> GetById(Guid id, CancellationToken cancellationToken);

    Task<MuscleGroup?> GetByName(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);

    Task<MuscleGroup> Add(MuscleGroup muscleGroup, CancellationToken cancellationToken);
}
