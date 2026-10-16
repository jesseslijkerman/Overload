namespace Overload.Api.Models.Requests;

public record CreateSetEntryRequest(
    int SessionId,
    int ExerciseId,
    int SetNumber,
    decimal Weight,
    int Reps,
    decimal? Rpe,
    bool IsWarmup);
