using Microsoft.AspNetCore.Mvc;
using Overload.Api.Services;
using Overload.Domain.Entities;

namespace Overload.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController (IExerciseService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Exercise>>> GetAllExercises(Guid userId)
    {
        var exercises = await service.GetAllAsync(userId);
        return exercises;
    }
}