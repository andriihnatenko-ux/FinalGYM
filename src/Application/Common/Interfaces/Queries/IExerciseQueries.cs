using Domain.Exercises;

namespace Application.Common.Interfaces.Queries;

public interface IExerciseQueries
{
    Task<IReadOnlyList<Exercise>> GetAll(CancellationToken cancellationToken);

    Task<Exercise?> GetById(Guid id, CancellationToken cancellationToken);
}
