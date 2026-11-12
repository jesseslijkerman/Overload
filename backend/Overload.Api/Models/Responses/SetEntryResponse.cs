using Overload.Domain.Entities;

namespace Overload.Api.Models.Responses;

/// <summary>
/// A single set. Has no session or exercise object, only ids: the client already knows
/// the session and can group sets per exercise using ExerciseId.
/// </summary>
public record SetEntryResponse(
    int Id,
    int ExerciseId,
    int SetNumber,
    decimal Weight,
    int Reps,
    decimal? Rpe,
    bool IsWarmup)
{
    public static SetEntryResponse From(SetEntry s) =>
        new(s.Id, s.ExerciseId, s.SetNumber, s.Weight, s.Reps, s.Rpe, s.IsWarmup);
}
