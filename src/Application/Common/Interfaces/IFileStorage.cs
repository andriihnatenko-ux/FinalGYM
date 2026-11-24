namespace Application.Common.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(
        Guid exerciseId,
        Guid imageId,
        string extension,
        Stream content,
        CancellationToken cancellationToken);
}
