using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.MuscleGroups;

namespace Application.Exercises.Commands;

internal static class ExerciseMuscleGroupGuard
{
    public static async Task EnsureExist(
        IMuscleGroupRepository muscleGroupRepository,
        IReadOnlyList<Guid> muscleGroupIds,
        CancellationToken cancellationToken)
    {
        var distinctIds = muscleGroupIds
            .Select(id => new MuscleGroupId(id))
            .Distinct()
            .ToList();

        var existingGroups = await muscleGroupRepository.GetByIds(distinctIds, cancellationToken);
        var existingIds = existingGroups.Select(x => x.Id).ToHashSet();
        var missingIds = distinctIds
            .Where(id => !existingIds.Contains(id))
            .Select(id => id.Value)
            .ToList();

        if (missingIds.Count > 0)
        {
            throw new MuscleGroupsNotFoundException(missingIds);
        }
    }
}
