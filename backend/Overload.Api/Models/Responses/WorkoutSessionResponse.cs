using Overload.Domain.Entities;

namespace Overload.Api.Models.Responses;

/// <summary>A session including its sets. Used for get-by-id, create and update.</summary>
public record WorkoutSessionResponse(
    int Id,
    DateOnly Date,
    string? Notes,
    IReadOnlyList<SetEntryResponse> Sets)
{
    public static WorkoutSessionResponse From(WorkoutSession s) =>
        new(s.Id, s.Date, s.Notes, s.Sets.Select(SetEntryResponse.From).ToList());
}

/// <summary>A session without its sets. Used for the list endpoint to keep it light.</summary>
public record WorkoutSessionSummaryResponse(int Id, DateOnly Date, string? Notes)
{
    public static WorkoutSessionSummaryResponse From(WorkoutSession s) => new(s.Id, s.Date, s.Notes);
}
