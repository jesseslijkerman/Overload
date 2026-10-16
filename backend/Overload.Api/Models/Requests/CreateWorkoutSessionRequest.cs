namespace Overload.Api.Models.Requests;

public record CreateWorkoutSessionRequest(
    DateOnly Date,
    string? Notes);
