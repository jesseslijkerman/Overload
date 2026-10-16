using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public interface IWorkoutSessionService
{
    Task<List<WorkoutSession>> GetAllAsync(Guid userId);
    Task<WorkoutSession?> GetByIdAsync(int id, Guid userId);
    Task<WorkoutSession> CreateAsync(CreateWorkoutSessionRequest request, Guid userId);
    Task<WorkoutSession?> UpdateAsync(int id, UpdateWorkoutSessionRequest request, Guid userId);
    Task<bool> DeleteAsync(int id, Guid userId);
}
