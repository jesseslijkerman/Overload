using System.ComponentModel.DataAnnotations;

namespace Overload.Api.Models.Requests;

public record UpdateExerciseRequest(
    [Required, StringLength(100, MinimumLength = 1)] string Name,
    [StringLength(50)] string? Category);
