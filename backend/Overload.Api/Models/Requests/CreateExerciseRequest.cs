namespace Overload.Api.Models.Requests;

public record CreateExerciseRequest(
    string Name,
    string? Category);
