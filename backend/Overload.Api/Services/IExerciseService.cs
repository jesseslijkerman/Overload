using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public interface IExerciseService
{
    Task<List<Exercise>> GetAllAsync(Guid userId);

    /// <exception cref="Exceptions.NotFoundException">Not found, or not visible to this user.</exception>
    Task<Exercise> GetByIdAsync(int id, Guid userId);

    /// <exception cref="Exceptions.ConflictException">An exercise with this name already exists.</exception>
    Task<Exercise> CreateAsync(CreateExerciseRequest request, Guid userId);

    /// <exception cref="Exceptions.NotFoundException">Not found, or not owned by this user.</exception>
    /// <exception cref="Exceptions.ConflictException">An exercise with this name already exists.</exception>
    Task<Exercise> UpdateAsync(int id, UpdateExerciseRequest request, Guid userId);

    /// <exception cref="Exceptions.NotFoundException">Not found, or not owned by this user.</exception>
    /// <exception cref="Exceptions.ConflictException">The exercise is used in logged sets.</exception>
    Task DeleteAsync(int id, Guid userId);
}
