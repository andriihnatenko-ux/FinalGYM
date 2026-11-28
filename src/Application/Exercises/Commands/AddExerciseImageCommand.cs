using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Exercises;
using MediatR;

namespace Application.Exercises.Commands;

public record AddExerciseImageCommand : IRequest<ExerciseImage?>
{
    public required Guid ExerciseId { get; init; }
    public required string OriginalName { get; init; }
    public required Stream Content { get; init; }
}

public class AddExerciseImageCommandHandler(
    IExerciseRepository exerciseRepository,
    IFileStorage fileStorage)
    : IRequestHandler<AddExerciseImageCommand, ExerciseImage?>
{
    public async Task<ExerciseImage?> Handle(
        AddExerciseImageCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseId = new ExerciseId(request.ExerciseId);
        var exercise = await exerciseRepository.GetById(exerciseId, cancellationToken);
        if (exercise is null)
        {
            return null;
        }

        var image = ExerciseImage.New(ExerciseImageId.New(), request.OriginalName, exerciseId);
        await exerciseRepository.AddImage(image, cancellationToken);

        var extension = Path.GetExtension(request.OriginalName);
        await fileStorage.SaveAsync(
            exerciseId,
            image.Id,
            extension,
            request.Content,
            cancellationToken);

        return image;
    }
}
