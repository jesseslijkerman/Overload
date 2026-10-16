namespace Overload.Api.Models.Requests;

public record UpdateExerciseRequest(
    string Name,
    string? Category);
