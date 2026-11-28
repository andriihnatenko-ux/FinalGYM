using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.MuscleGroups.Commands;
using Application.MuscleGroups.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("muscle-groups")]
[ApiController]
public class MuscleGroupsController(ISender sender, IMuscleGroupQueries muscleGroupQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MuscleGroupDto>>> GetMuscleGroups(CancellationToken cancellationToken)
    {
        var muscleGroups = await muscleGroupQueries.GetAll(cancellationToken);
        return Ok(muscleGroups.Select(MuscleGroupDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MuscleGroupDto>> GetMuscleGroup(Guid id, CancellationToken cancellationToken)
    {
        var muscleGroup = await muscleGroupQueries.GetById(id, cancellationToken);
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
        var command = new CreateMuscleGroupCommand { Name = request.Name };

        try
        {
            var muscleGroup = await sender.Send(command, cancellationToken);
            var dto = MuscleGroupDto.FromDomainModel(muscleGroup);
            return CreatedAtAction(nameof(GetMuscleGroup), new { id = dto.Id }, dto);
        }
        catch (MuscleGroupAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }
}
