using Domain.MuscleGroups;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IMuscleGroupRepository
{
    Task<Option<MuscleGroup>> GetById(MuscleGroupId id, CancellationToken cancellationToken);

    Task<Option<MuscleGroup>> GetByName(string name, CancellationToken cancellationToken);

    Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<MuscleGroupId> ids,
        CancellationToken cancellationToken);

    Task<MuscleGroup> Add(MuscleGroup muscleGroup, CancellationToken cancellationToken);
}
