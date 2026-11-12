using System.ComponentModel.DataAnnotations;

namespace Overload.Api.Models.Requests;

/// <summary>
/// A set to create. Used both nested in <see cref="CreateWorkoutSessionRequest"/>
/// and for adding a single set to an existing session, where the session id comes from the route.
/// </summary>
public record CreateSetEntryRequest(
    int ExerciseId,
    [Range(1, 100)] int SetNumber,
    [Range(0.0, 1000.0)] decimal Weight,
    [Range(0, 1000)] int Reps,
    [Range(0.0, 10.0)] decimal? Rpe,
    bool IsWarmup);
