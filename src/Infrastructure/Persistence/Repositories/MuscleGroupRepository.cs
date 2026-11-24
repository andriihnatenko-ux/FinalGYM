using Application.Common.Interfaces;
using Domain.MuscleGroups;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository(ApplicationDbContext context) : IMuscleGroupRepository
{
    public async Task<IReadOnlyList<MuscleGroup>> GetAll(CancellationToken cancellationToken)
    {
        return await context.MuscleGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<MuscleGroup?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.MuscleGroups
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MuscleGroup?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.MuscleGroups
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken)
    {
        return await context.MuscleGroups
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<MuscleGroup> Add(MuscleGroup muscleGroup, CancellationToken cancellationToken)
    {
        await context.MuscleGroups.AddAsync(muscleGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return muscleGroup;
    }
}
