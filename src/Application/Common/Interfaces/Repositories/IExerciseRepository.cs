using Domain.Exercises;

namespace Application.Common.Interfaces.Repositories;

public interface IExerciseRepository
{
    Task<Exercise?> GetById(Guid id, CancellationToken cancellationToken);

    Task<Exercise?> GetByTitle(string title, CancellationToken cancellationToken);

    Task<Exercise> Add(Exercise exercise, CancellationToken cancellationToken);

    Task<Exercise> Update(Exercise exercise, CancellationToken cancellationToken);

    Task Delete(Exercise exercise, CancellationToken cancellationToken);

    Task AddImage(ExerciseImage image, CancellationToken cancellationToken);
}
