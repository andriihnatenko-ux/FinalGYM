using Api.Dtos;
using Application.MuscleGroups.Exceptions;
using Application.MuscleGroups.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("muscle-groups")]
[ApiController]
public class MuscleGroupsController(IMuscleGroupService muscleGroupService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MuscleGroupDto>>> GetMuscleGroups(CancellationToken cancellationToken)
    {
        var muscleGroups = await muscleGroupService.GetMuscleGroups(cancellationToken);
        return Ok(muscleGroups.Select(MuscleGroupDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MuscleGroupDto>> GetMuscleGroup(Guid id, CancellationToken cancellationToken)
    {
        var muscleGroup = await muscleGroupService.GetMuscleGroup(id, cancellationToken);
        if (muscleGroup is null)
        {
            return NotFound();
        }

        return MuscleGroupDto.FromDomainModel(muscleGroup);
    }

    [HttpPost]
    public async Task<ActionResult<MuscleGroupDto>> CreateMuscleGroup(
        [FromBody] CreateMuscleGroupDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var muscleGroup = await muscleGroupService.Add(request.Name, cancellationToken);
            var dto = MuscleGroupDto.FromDomainModel(muscleGroup);
            return CreatedAtAction(nameof(GetMuscleGroup), new { id = dto.Id }, dto);
        }
        catch (MuscleGroupAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }
}
