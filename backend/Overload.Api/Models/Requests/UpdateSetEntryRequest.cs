using System.ComponentModel.DataAnnotations;

namespace Overload.Api.Models.Requests;

/// <summary>
/// Updates the values of a set. The exercise and the session of a set cannot be changed:
/// delete the set and create a new one instead.
/// </summary>
public record UpdateSetEntryRequest(
    [Range(1, 100)] int SetNumber,
    [Range(0.0, 1000.0)] decimal Weight,
    [Range(0, 1000)] int Reps,
    [Range(0.0, 10.0)] decimal? Rpe,
    bool IsWarmup);
