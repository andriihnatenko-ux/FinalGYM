using Application.Common.Interfaces.Repositories;
using Domain.MuscleGroups;

namespace Application.Exercises.Commands;

internal static class ExerciseMuscleGroupGuard
{
    public static async Task<IReadOnlyList<MuscleGroupId>> FindMissing(
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

        return distinctIds.Where(id => !existingIds.Contains(id)).ToList();
    }
}
