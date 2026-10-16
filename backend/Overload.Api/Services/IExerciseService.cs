using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public interface IExerciseService
{
    Task<List<Exercise>> GetAllAsync(Guid userId);
    Task<Exercise?> GetByIdAsync(int id);
    Task<Exercise> CreateAsync(CreateExerciseRequest request, Guid userId);
    Task<Exercise?> UpdateAsync(int id, UpdateExerciseRequest request, Guid userId);
    Task<bool> DeleteAsync(int id, Guid userId);
}
