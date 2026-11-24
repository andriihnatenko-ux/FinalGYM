using Domain.Exercises;

namespace Application.Common.Interfaces;

public interface IExerciseRepository
{
    Task<IReadOnlyList<Exercise>> GetAll(CancellationToken cancellationToken);

    Task<Exercise?> GetById(Guid id, CancellationToken cancellationToken);

    Task<Exercise?> GetByName(string name, CancellationToken cancellationToken);

    Task<Exercise> Add(Exercise exercise, CancellationToken cancellationToken);

    Task<Exercise> Update(Exercise exercise, CancellationToken cancellationToken);

    Task Delete(Exercise exercise, CancellationToken cancellationToken);
}
