using System.ComponentModel.DataAnnotations;

namespace Overload.Api.Models.Requests;

// For records, validation attributes go on the constructor parameter (not with 'property:').
public record CreateExerciseRequest(
    [Required, StringLength(100, MinimumLength = 1)] string Name,
    [StringLength(50)] string? Category);
