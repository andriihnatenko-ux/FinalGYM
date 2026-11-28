using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Exercises.Commands;
using Application.Exercises.Exceptions;
using Domain.Exercises;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("exercises")]
[ApiController]
public class ExercisesController(ISender sender, IExerciseQueries exerciseQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetExercises(CancellationToken cancellationToken)
    {
        var exercises = await exerciseQueries.GetAll(cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> GetExercise(Guid id, CancellationToken cancellationToken)
    {
        var exerciseOption = await exerciseQueries.GetById(new ExerciseId(id), cancellationToken);
        return exerciseOption.Match<ActionResult<ExerciseDto>>(
            exercise => ExerciseDto.FromDomainModel(exercise),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> CreateExercise(
        [FromBody] CreateExerciseDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateExerciseCommand
        {
            Title = request.Title,
            Description = request.Description,
            CaloriesPerHour = request.CaloriesPerHour,
            RequiresEquipment = request.RequiresEquipment,
            Sets = request.Sets,
            Reps = request.Reps,
            MuscleGroupIds = request.MuscleGroupIds,
            Attributes = request.Attributes?.ToDomainModel()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ExerciseDto>>(
            exercise =>
            {
                var dto = ExerciseDto.FromDomainModel(exercise);
                return CreatedAtAction(nameof(GetExercise), new { id = dto.Id }, dto);
            }, error => ToErrorResult(error));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> UpdateExercise(
        Guid id,
        [FromBody] UpdateExerciseDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateExerciseCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            CaloriesPerHour = request.CaloriesPerHour,
            RequiresEquipment = request.RequiresEquipment,
            Sets = request.Sets,
            Reps = request.Reps,
            MuscleGroupIds = request.MuscleGroupIds,
            Attributes = request.Attributes?.ToDomainModel()
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ExerciseDto>>(
            exercise => ExerciseDto.FromDomainModel(exercise), error => ToErrorResult(error));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExercise(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteExerciseCommand { Id = id };
        var result = await sender.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            _ => NoContent(), error => ToErrorResult(error));
    }

    [HttpPost("{id:guid}/images")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ExerciseImageDto>> UploadImage(
        Guid id,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        await using var content = file.OpenReadStream();
        var command = new AddExerciseImageCommand
        {
            ExerciseId = id,
            OriginalName = file.FileName,
            Content = content
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<ExerciseImageDto>>(
            image => Created(
                $"/exercises/{id}/images/{image.Id.Value}",
                ExerciseImageDto.FromDomainModel(image)), error => ToErrorResult(error));
    }

    private ActionResult ToErrorResult(ExerciseException exception) => exception switch
    {
        ExerciseAlreadyExistsException => Conflict(exception.Message),
        ExerciseNotFoundException => NotFound(exception.Message),
        ExerciseGroupsNotFoundException groupsNotFound => NotFound(new
        {
            message = groupsNotFound.Message,
            missingGroupIds = groupsNotFound.MissingGroupIds.Select(id => id.Value)
        }),
        ExerciseUnhandledException => StatusCode(
            StatusCodes.Status500InternalServerError,
            exception.Message),
        _ => StatusCode(StatusCodes.Status500InternalServerError, exception.Message)
    };
}

