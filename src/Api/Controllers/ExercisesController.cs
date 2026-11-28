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
        var exercise = await exerciseQueries.GetById(new ExerciseId(id), cancellationToken);
        if (exercise is null)
        {
            return NotFound();
        }

        return ExerciseDto.FromDomainModel(exercise);
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

        try
        {
            var exercise = await sender.Send(command, cancellationToken);
            var dto = ExerciseDto.FromDomainModel(exercise);
            return CreatedAtAction(nameof(GetExercise), new { id = dto.Id }, dto);
        }
        catch (ExerciseAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
        catch (MuscleGroupsNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message,
                missingGroupIds = exception.MissingGroupIds
            });
        }
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

        try
        {
            var exercise = await sender.Send(command, cancellationToken);
            if (exercise is null)
            {
                return NotFound();
            }

            return ExerciseDto.FromDomainModel(exercise);
        }
        catch (ExerciseAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
        catch (MuscleGroupsNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message,
                missingGroupIds = exception.MissingGroupIds
            });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExercise(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteExerciseCommand { Id = id };
        var deleted = await sender.Send(command, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
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

        var image = await sender.Send(command, cancellationToken);
        if (image is null)
        {
            return NotFound();
        }

        return Created($"/exercises/{id}/images/{image.Id.Value}", ExerciseImageDto.FromDomainModel(image));
    }
}
