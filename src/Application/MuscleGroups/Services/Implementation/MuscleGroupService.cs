using Application.Common.Interfaces;
using Application.MuscleGroups.Exceptions;
using Application.MuscleGroups.Services.Abstract;
using Domain.MuscleGroups;

namespace Application.MuscleGroups.Services.Implementation;

public class MuscleGroupService(IMuscleGroupRepository muscleGroupRepository) : IMuscleGroupService
{
    public async Task<IReadOnlyList<MuscleGroup>> GetMuscleGroups(CancellationToken cancellationToken)
    {
        return await muscleGroupRepository.GetAll(cancellationToken);
    }

    public async Task<MuscleGroup?> GetMuscleGroup(Guid id, CancellationToken cancellationToken)
    {
        return await muscleGroupRepository.GetById(id, cancellationToken);
    }

    public async Task<MuscleGroup> Add(string name, CancellationToken cancellationToken)
    {
        var existingMuscleGroup = await muscleGroupRepository.GetByName(name, cancellationToken);
        if (existingMuscleGroup is not null)
        {
            throw new MuscleGroupAlreadyExistsException(name);
        }

        var muscleGroup = MuscleGroup.New(Guid.NewGuid(), name);
        return await muscleGroupRepository.Add(muscleGroup, cancellationToken);
    }
}
