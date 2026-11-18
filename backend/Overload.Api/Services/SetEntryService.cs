using Microsoft.EntityFrameworkCore;
using Overload.Api.Data;
using Overload.Api.Exceptions;
using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public class SetEntryService(OverloadDbContext db) : ISetEntryService
{
    // A set has no owner of its own: it belongs to the user who owns its session
    private IQueryable<SetEntry> OwnedBy(Guid userId) =>
        db.SetEntries.Where(s => s.Session.UserId == userId);

    public async Task<SetEntry> GetByIdAsync(int id, Guid userId)
    {
        var set = await OwnedBy(userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        return set ?? throw new NotFoundException($"Set {id} was not found.");
    }

    public async Task<List<SetEntry>> GetBySessionAsync(int sessionId, Guid userId)
    {
        await EnsureSessionIsOwnedAsync(sessionId, userId);

        return await db.SetEntries
            .AsNoTracking()
            .Where(s => s.SessionId == sessionId)
            .OrderBy(s => s.ExerciseId)
            .ThenBy(s => s.SetNumber)
            .ToListAsync();
    }

    public async Task<List<SetEntry>> GetByExerciseAsync(int exerciseId, Guid userId)
    {
        await EnsureExerciseIsVisibleAsync(exerciseId, userId);

        return await OwnedBy(userId)
            .AsNoTracking()
            .Include(s => s.Session)
            .Where(s => s.ExerciseId == exerciseId)
            .OrderBy(s => s.Session.Date)
            .ThenBy(s => s.SessionId)
            .ThenBy(s => s.SetNumber)
            .ToListAsync();
    }

    public async Task<SetEntry> CreateAsync(int sessionId, CreateSetEntryRequest request, Guid userId)
    {
        await EnsureSessionIsOwnedAsync(sessionId, userId);
        await EnsureExerciseIsVisibleAsync(request.ExerciseId, userId);
        await EnsureSetNumberIsFreeAsync(sessionId, request.ExerciseId, request.SetNumber, excludeId: null);

        var set = new SetEntry
        {
            SessionId = sessionId,
            ExerciseId = request.ExerciseId,
            SetNumber = request.SetNumber,
            Weight = request.Weight,
            Reps = request.Reps,
            Rpe = request.Rpe,
            IsWarmup = request.IsWarmup
        };

        db.SetEntries.Add(set);
        await db.SaveChangesAsync();
        return set;
    }

    public async Task<SetEntry> UpdateAsync(int id, UpdateSetEntryRequest request, Guid userId)
    {
        var set = await OwnedBy(userId).FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException($"Set {id} was not found.");

        if (set.SetNumber != request.SetNumber)
            await EnsureSetNumberIsFreeAsync(set.SessionId, set.ExerciseId, request.SetNumber, excludeId: id);

        set.SetNumber = request.SetNumber;
        set.Weight = request.Weight;
        set.Reps = request.Reps;
        set.Rpe = request.Rpe;
        set.IsWarmup = request.IsWarmup;

        await db.SaveChangesAsync();
        return set;
    }

    public async Task DeleteAsync(int id, Guid userId)
    {
        var set = await OwnedBy(userId).FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException($"Set {id} was not found.");

        db.SetEntries.Remove(set);
        await db.SaveChangesAsync();
    }

    private async Task EnsureSessionIsOwnedAsync(int sessionId, Guid userId)
    {
        var owned = await db.WorkoutSessions.AnyAsync(x => x.Id == sessionId && x.UserId == userId);
        if (!owned)
            throw new NotFoundException($"Workout Session {sessionId} was not found.");
    }

    // Own custom exercises and the default exercises (CreatedByUserId == null)
    private async Task EnsureExerciseIsVisibleAsync(int exerciseId, Guid userId)
    {
        var visible = await db.Exercises.AnyAsync(e =>
            e.Id == exerciseId && (e.CreatedByUserId == userId || e.CreatedByUserId == null));
        if (!visible)
            throw new NotFoundException($"Exercise {exerciseId} was not found.");
    }

    private async Task EnsureSetNumberIsFreeAsync(int sessionId, int exerciseId, int setNumber, int? excludeId)
    {
        var taken = await db.SetEntries.AnyAsync(s =>
            s.SessionId == sessionId
            && s.ExerciseId == exerciseId
            && s.SetNumber == setNumber
            && (excludeId == null || s.Id != excludeId));
        if (taken)
            throw new ConflictException($"Set number {setNumber} already exists for exercise {exerciseId} in this session.");
    }
}
