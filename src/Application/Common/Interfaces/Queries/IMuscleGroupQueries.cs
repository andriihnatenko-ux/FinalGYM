using Domain.MuscleGroups;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IMuscleGroupQueries
{
    Task<IReadOnlyList<MuscleGroup>> GetAll(CancellationToken cancellationToken);

    Task<Option<MuscleGroup>> GetById(MuscleGroupId id, CancellationToken cancellationToken);
}
