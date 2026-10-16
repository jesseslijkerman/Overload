using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public interface ISetEntryService
{
    Task<List<SetEntry>> GetBySessionAsync(int sessionId, Guid userId);
    Task<List<SetEntry>> GetByExerciseAsync(int exerciseId, Guid userId);
    Task<SetEntry> CreateAsync(CreateSetEntryRequest request, Guid userId);
    Task<SetEntry?> UpdateAsync(int id, UpdateSetEntryRequest request, Guid userId);
    Task<bool> DeleteAsync(int id, Guid userId);
}
