using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public interface ISetEntryService
{
    /// <exception cref="Exceptions.NotFoundException">The set does not exist or belongs to another user.</exception>
    Task<SetEntry> GetByIdAsync(int id, Guid userId);

    /// <summary>Gets the sets of one session, ordered by exercise and set number.</summary>
    /// <exception cref="Exceptions.NotFoundException">The session does not exist or belongs to another user.</exception>
    Task<List<SetEntry>> GetBySessionAsync(int sessionId, Guid userId);

    /// <summary>Gets all sets of the user for one exercise, oldest session first, with the session loaded.</summary>
    /// <exception cref="Exceptions.NotFoundException">The exercise does not exist or is not visible to the user.</exception>
    Task<List<SetEntry>> GetByExerciseAsync(int exerciseId, Guid userId);

    /// <exception cref="Exceptions.NotFoundException">The session or the exercise was not found.</exception>
    /// <exception cref="Exceptions.ConflictException">The session already has this set number for this exercise.</exception>
    Task<SetEntry> CreateAsync(int sessionId, CreateSetEntryRequest request, Guid userId);

    /// <exception cref="Exceptions.NotFoundException">The set does not exist or belongs to another user.</exception>
    /// <exception cref="Exceptions.ConflictException">The new set number is already used for this exercise in this session.</exception>
    Task<SetEntry> UpdateAsync(int id, UpdateSetEntryRequest request, Guid userId);

    /// <exception cref="Exceptions.NotFoundException">The set does not exist or belongs to another user.</exception>
    Task DeleteAsync(int id, Guid userId);
}
