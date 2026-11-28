using Application.Common.Interfaces;
using Domain.Exercises;

namespace Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _root = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

    public async Task<string> SaveAsync(
        ExerciseId exerciseId,
        ExerciseImageId imageId,
        string extension,
        Stream content,
        CancellationToken cancellationToken)
    {
        var exerciseDirectory = Path.Combine(_root, exerciseId.Value.ToString());
        Directory.CreateDirectory(exerciseDirectory);

        var fileName = $"{imageId.Value}{extension}";
        var fullPath = Path.Combine(exerciseDirectory, fileName);

        await using var file = File.Create(fullPath);
        await content.CopyToAsync(file, cancellationToken);

        return $"{exerciseId.Value}/{fileName}";
    }
}
