using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.MuscleGroups;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository(ApplicationDbContext context) : IMuscleGroupRepository, IMuscleGroupQueries
{
    public async Task<IReadOnlyList<MuscleGroup>> GetAll(CancellationToken cancellationToken)
    {
        return await context.MuscleGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<MuscleGroup>> GetById(MuscleGroupId id, CancellationToken cancellationToken)
    {
        var entity = await context.MuscleGroups
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? Option<MuscleGroup>.None : entity;
    }

    public async Task<Option<MuscleGroup>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.MuscleGroups
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity is null ? Option<MuscleGroup>.None : entity;
    }

    public async Task<IReadOnlyList<MuscleGroup>> GetByIds(
        IReadOnlyCollection<MuscleGroupId> ids,
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
