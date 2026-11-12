using Microsoft.AspNetCore.Mvc;
using Overload.Api.Models.Requests;
using Overload.Api.Models.Responses;
using Overload.Api.Services;

namespace Overload.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutSessionController(IWorkoutSessionService service, ICurrentUser user) : ControllerBase
{
    /// <summary>Lists the current user's sessions, newest first, without their sets.</summary>
    [HttpGet]
    [ProducesResponseType<List<WorkoutSessionSummaryResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkoutSessionSummaryResponse>>> GetAllWorkoutSessions()
    {
        var sessions = await service.GetAllAsync(user.Id);
        return Ok(sessions.Select(WorkoutSessionSummaryResponse.From).ToList());
    }

    /// <summary>Gets one session including its sets, ordered by exercise and set number.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<WorkoutSessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponse>> GetWorkoutSessionById(int id)
    {
        var session = await service.GetByIdAsync(id, user.Id);
        return Ok(WorkoutSessionResponse.From(session));
    }

    /// <summary>Creates a session, optionally with its sets in the same request.</summary>
    [HttpPost]
    [ProducesResponseType<WorkoutSessionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponse>> CreateWorkoutSession(
        [FromBody] CreateWorkoutSessionRequest request)
    {
        var created = await service.CreateAsync(request, user.Id);

        // 201 with a Location header pointing to GET api/workoutsession/{id}
        return CreatedAtAction(
            nameof(GetWorkoutSessionById),
            new { id = created.Id },
            WorkoutSessionResponse.From(created));
    }

    /// <summary>Updates the date and notes of a session. Sets are managed separately.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType<WorkoutSessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponse>> UpdateWorkoutSession(
        int id, [FromBody] UpdateWorkoutSessionRequest request)
    {
        var updated = await service.UpdateAsync(id, request, user.Id);
        return Ok(WorkoutSessionResponse.From(updated));
    }

    /// <summary>Deletes a session and all of its sets.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWorkoutSession(int id)
    {
        await service.DeleteAsync(id, user.Id);
        return NoContent();
    }
}
