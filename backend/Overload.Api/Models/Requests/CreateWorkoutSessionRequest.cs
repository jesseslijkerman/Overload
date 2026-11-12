using System.ComponentModel.DataAnnotations;

namespace Overload.Api.Models.Requests;

/// <summary>
/// Creates a workout session, optionally with its sets in one go.
/// Sets is optional: a user can start an empty session and add sets one by one later.
/// </summary>
public record CreateWorkoutSessionRequest(
    DateOnly Date,
    [StringLength(2000)] string? Notes,
    [MaxLength(200)] List<CreateSetEntryRequest>? Sets);
