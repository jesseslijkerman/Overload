namespace Overload.Api.Models.Requests;

public record UpdateSetEntryRequest(
    int SetNumber,
    decimal Weight,
    int Reps,
    decimal? Rpe,
    bool IsWarmup);
