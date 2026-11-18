using Overload.Domain.Entities;

namespace Overload.Api.Models.Responses;

/// <summary>
/// A set together with the date of its session. Used for the history of one exercise,
/// which is the data behind progressive overload graphs.
/// </summary>
public record SetHistoryResponse(
    int Id,
    int SessionId,
    DateOnly Date,
    int SetNumber,
    decimal Weight,
    int Reps,
    decimal? Rpe,
    bool IsWarmup)
{
    /// <remarks>Requires <see cref="SetEntry.Session"/> to be loaded.</remarks>
    public static SetHistoryResponse From(SetEntry s) =>
        new(s.Id, s.SessionId, s.Session.Date, s.SetNumber, s.Weight, s.Reps, s.Rpe, s.IsWarmup);
}
