using Domain.Exercises;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IExerciseQueries
{
    Task<IReadOnlyList<Exercise>> GetAll(CancellationToken cancellationToken);

    Task<Option<Exercise>> GetById(ExerciseId id, CancellationToken cancellationToken);
}
