using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Exercises;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ExerciseRepository(ApplicationDbContext context) : IExerciseRepository, IExerciseQueries
{
    public async Task<IReadOnlyList<Exercise>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Exercises
            .AsNoTracking()
            .Include(x => x.MuscleGroups)
            .ThenInclude(x => x.MuscleGroup)
            .Include(x => x.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Exercise>> GetById(ExerciseId id, CancellationToken cancellationToken)
    {
        var entity = await context.Exercises
            .Include(x => x.MuscleGroups)
            .ThenInclude(x => x.MuscleGroup)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? Option<Exercise>.None : entity;
    }

    public async Task<Option<Exercise>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Exercises
            .FirstOrDefaultAsync(x => x.Title == title, cancellationToken);

        return entity is null ? Option<Exercise>.None : entity;
    }

    public async Task<Exercise> Add(Exercise exercise, CancellationToken cancellationToken)
    {
        await context.Exercises.AddAsync(exercise, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return exercise;
    }

    public async Task<Exercise> Update(Exercise exercise, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (context.Entry(exercise).State == EntityState.Detached)
            {
                context.Exercises.Update(exercise);
            }

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return exercise;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task Delete(Exercise exercise, CancellationToken cancellationToken)
    {
        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddImage(ExerciseImage image, CancellationToken cancellationToken)
    {
        await context.ExerciseImages.AddAsync(image, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
