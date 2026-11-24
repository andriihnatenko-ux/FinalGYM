using Application.Common.Interfaces;
using Domain.Exercises;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ExerciseRepository(ApplicationDbContext context) : IExerciseRepository
{
    public async Task<IReadOnlyList<Exercise>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Exercises
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Exercise?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Exercises
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Exercise?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Exercises
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<Exercise> Add(Exercise exercise, CancellationToken cancellationToken)
    {
        await context.Exercises.AddAsync(exercise, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return exercise;
    }

    public async Task<Exercise> Update(Exercise exercise, CancellationToken cancellationToken)
    {
        context.Exercises.Update(exercise);
        await context.SaveChangesAsync(cancellationToken);
        return exercise;
    }

    public async Task Delete(Exercise exercise, CancellationToken cancellationToken)
    {
        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync(cancellationToken);
    }
}
