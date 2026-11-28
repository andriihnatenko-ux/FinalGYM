using Domain.Exercises;

namespace Application.Common.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(
        ExerciseId exerciseId,
        ExerciseImageId imageId,
        string extension,
        Stream content,
        CancellationToken cancellationToken);
}
