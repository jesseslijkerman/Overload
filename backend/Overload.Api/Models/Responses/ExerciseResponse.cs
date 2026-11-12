using Overload.Domain.Entities;

namespace Overload.Api.Models.Responses;

/// <summary>An exercise as returned by the API. Does not expose CreatedByUserId.</summary>
public record ExerciseResponse(int Id, string Name, string? Category, bool IsCustom)
{
    public static ExerciseResponse From(Exercise e) => new(e.Id, e.Name, e.Category, e.IsCustom);
}
