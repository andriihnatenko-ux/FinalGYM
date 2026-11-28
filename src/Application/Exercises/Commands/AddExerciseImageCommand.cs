using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using LanguageExt;
using MediatR;

namespace Application.Exercises.Commands;

public record AddExerciseImageCommand : IRequest<Either<ExerciseException, ExerciseImage>>
{
    public required Guid ExerciseId { get; init; }
    public required string OriginalName { get; init; }
    public required Stream Content { get; init; }
}

public class AddExerciseImageCommandHandler(
    IExerciseRepository exerciseRepository,
    IFileStorage fileStorage)
    : IRequestHandler<AddExerciseImageCommand, Either<ExerciseException, ExerciseImage>>
{
    public async Task<Either<ExerciseException, ExerciseImage>> Handle(
        AddExerciseImageCommand request,
        CancellationToken cancellationToken)
    {
        var exerciseId = new ExerciseId(request.ExerciseId);
        var exerciseOption = await exerciseRepository.GetById(exerciseId, cancellationToken);

        return await exerciseOption.Match<Task<Either<ExerciseException, ExerciseImage>>>(
            Some: _ => AddImage(exerciseId, request, cancellationToken),
            None: () => Task.FromResult<Either<ExerciseException, ExerciseImage>>(
                new ExerciseNotFoundException(exerciseId)));
    }

    private async Task<Either<ExerciseException, ExerciseImage>> AddImage(
        ExerciseId exerciseId,
        AddExerciseImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
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
        catch (Exception exception)
        {
            return new ExerciseUnhandledException(exerciseId, exception);
        }
    }
}
