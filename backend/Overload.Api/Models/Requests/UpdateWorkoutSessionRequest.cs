namespace Overload.Api.Models.Requests;

public record UpdateWorkoutSessionRequest(
    DateOnly Date,
    string? Notes);
