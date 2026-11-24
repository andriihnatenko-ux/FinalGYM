using Api.Dtos;
using Application.Exercises.Exceptions;
using Application.Exercises.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("exercises")]
[ApiController]
public class ExercisesController(IExerciseService exerciseService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExerciseDto>>> GetExercises(CancellationToken cancellationToken)
    {
        var exercises = await exerciseService.GetExercises(cancellationToken);
        return Ok(exercises.Select(ExerciseDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> GetExercise(Guid id, CancellationToken cancellationToken)
    {
        var exercise = await exerciseService.GetExercise(id, cancellationToken);
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
        try
        {
            var exercise = await exerciseService.Add(
                request.Title,
                request.Description,
                request.CaloriesPerHour,
                request.RequiresEquipment,
                request.Sets,
                request.Reps,
                request.MuscleGroupIds,
                request.Attributes?.ToDomainModel(),
                cancellationToken);

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
        catch (InvalidExerciseRequestException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> UpdateExercise(
        Guid id,
        [FromBody] UpdateExerciseDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var exercise = await exerciseService.Update(
                id,
                request.Title,
                request.Description,
                request.CaloriesPerHour,
                request.RequiresEquipment,
                request.Sets,
                request.Reps,
                request.MuscleGroupIds,
                request.Attributes?.ToDomainModel(),
                cancellationToken);

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
        catch (InvalidExerciseRequestException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExercise(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await exerciseService.Delete(id, cancellationToken);
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
        var image = await exerciseService.AddImage(id, file.FileName, content, cancellationToken);
        if (image is null)
        {
            return NotFound();
        }

        return Created($"/exercises/{id}/images/{image.Id}", ExerciseImageDto.FromDomainModel(image));
    }
}
