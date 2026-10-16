using Microsoft.AspNetCore.Mvc;
using Overload.Api.Models.Requests;
using Overload.Api.Services;
using Overload.Domain.Entities;

namespace Overload.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController(IExerciseService service, ICurrentUser user) : ControllerBase
{
    /// <summary>Lists the default exercises plus the current user's own exercises.</summary>
    [HttpGet]
    [ProducesResponseType<List<Exercise>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Exercise>>> GetAllExercises()
    {
        var exercises = await service.GetAllAsync(user.Id);
        return Ok(exercises);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Exercise>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Exercise>> GetExerciseById(int id)
    {
        var exercise = await service.GetByIdAsync(id, user.Id);
        return Ok(exercise);
    }

    [HttpPost]
    [ProducesResponseType<Exercise>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Exercise>> CreateExercise([FromBody] CreateExerciseRequest request)
    {
        var created = await service.CreateAsync(request, user.Id);

        // 201 with a Location header pointing to GET api/exercise/{id}
        return CreatedAtAction(nameof(GetExerciseById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<Exercise>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Exercise>> UpdateExercise(int id, [FromBody] UpdateExerciseRequest request)
    {
        var updated = await service.UpdateAsync(id, request, user.Id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        await service.DeleteAsync(id, user.Id);
        return NoContent();
    }
}
