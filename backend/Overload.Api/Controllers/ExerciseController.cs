using Microsoft.AspNetCore.Mvc;
using Overload.Api.Models.Requests;
using Overload.Api.Services;
using Overload.Domain.Entities;

namespace Overload.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController (IExerciseService service, ICurrentUser user) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Exercise>>> GetAllExercises()
    {
        var exercises = await service.GetAllAsync(user.Id);
        return exercises;
    }

    [HttpPost]
    public async Task<ActionResult<Exercise>> CreateExercise([FromBody] CreateExerciseRequest exercise)
    {
        return await service.CreateAsync(exercise, user.Id);
    }
}