using Microsoft.AspNetCore.Mvc;
using Overload.Api.Models.Requests;
using Overload.Api.Models.Responses;
using Overload.Api.Services;

namespace Overload.Api.Controllers;

/// <summary>
/// Sets are reached through their parent resources (a session or an exercise) when listing or creating,
/// and by their own id when reading, updating or deleting a single set.
/// </summary>
[ApiController]
[Route("api")]
public class SetEntryController(ISetEntryService service, ICurrentUser user) : ControllerBase
{
    /// <summary>Lists the sets of one session, ordered by exercise and set number.</summary>
    [HttpGet("workoutsession/{sessionId:int}/sets")]
    [ProducesResponseType<List<SetEntryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<SetEntryResponse>>> GetSetsBySession(int sessionId)
    {
        var sets = await service.GetBySessionAsync(sessionId, user.Id);
        return Ok(sets.Select(SetEntryResponse.From).ToList());
    }

    /// <summary>Adds one set to an existing session.</summary>
    [HttpPost("workoutsession/{sessionId:int}/sets")]
    [ProducesResponseType<SetEntryResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SetEntryResponse>> CreateSet(
        int sessionId, [FromBody] CreateSetEntryRequest request)
    {
        var created = await service.CreateAsync(sessionId, request, user.Id);

        // 201 with a Location header pointing to GET api/setentry/{id}
        return CreatedAtAction(
            nameof(GetSetById),
            new { id = created.Id },
            SetEntryResponse.From(created));
    }

    /// <summary>Gets the history of one exercise (all sets with their session date), oldest first.</summary>
    [HttpGet("exercise/{exerciseId:int}/sets")]
    [ProducesResponseType<List<SetHistoryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<SetHistoryResponse>>> GetSetHistoryByExercise(int exerciseId)
    {
        var sets = await service.GetByExerciseAsync(exerciseId, user.Id);
        return Ok(sets.Select(SetHistoryResponse.From).ToList());
    }

    [HttpGet("setentry/{id:int}")]
    [ProducesResponseType<SetEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SetEntryResponse>> GetSetById(int id)
    {
        var set = await service.GetByIdAsync(id, user.Id);
        return Ok(SetEntryResponse.From(set));
    }

    /// <summary>Updates the values of a set. Its session and exercise cannot be changed.</summary>
    [HttpPut("setentry/{id:int}")]
    [ProducesResponseType<SetEntryResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SetEntryResponse>> UpdateSet(int id, [FromBody] UpdateSetEntryRequest request)
    {
        var updated = await service.UpdateAsync(id, request, user.Id);
        return Ok(SetEntryResponse.From(updated));
    }

    [HttpDelete("setentry/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSet(int id)
    {
        await service.DeleteAsync(id, user.Id);
        return NoContent();
    }
}
