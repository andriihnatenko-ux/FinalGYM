using Domain.Exercises;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IExerciseRepository
{
    Task<Option<Exercise>> GetById(ExerciseId id, CancellationToken cancellationToken);

    Task<Option<Exercise>> GetByTitle(string title, CancellationToken cancellationToken);

    Task<Exercise> Add(Exercise exercise, CancellationToken cancellationToken);

    Task<Exercise> Update(Exercise exercise, CancellationToken cancellationToken);

    Task Delete(Exercise exercise, CancellationToken cancellationToken);

    Task AddImage(ExerciseImage image, CancellationToken cancellationToken);
}
