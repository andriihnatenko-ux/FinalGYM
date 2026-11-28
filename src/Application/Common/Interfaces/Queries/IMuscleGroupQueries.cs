using Domain.MuscleGroups;

namespace Application.Common.Interfaces.Queries;

public interface IMuscleGroupQueries
{
    Task<IReadOnlyList<MuscleGroup>> GetAll(CancellationToken cancellationToken);

    Task<MuscleGroup?> GetById(MuscleGroupId id, CancellationToken cancellationToken);
}
