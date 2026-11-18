using Microsoft.EntityFrameworkCore;
using Overload.Api.Data;
using Overload.Api.Exceptions;
using Overload.Api.Models.Requests;

namespace Overload.Api.Services;

using Overload.Domain.Entities;

public class WorkoutSessionService(OverloadDbContext db) : IWorkoutSessionService
{
    // Only a user's own sessions may be viewed, updated or deleted
    private IQueryable<WorkoutSession> OwnedBy(Guid userId) =>
        db.WorkoutSessions.Where(x => x.UserId == userId);
    
    public async Task<List<WorkoutSession>> GetAllAsync(Guid userId)
    {
        return await OwnedBy(userId)
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<WorkoutSession> GetByIdAsync(int id, Guid userId)
    {
        var session = await OwnedBy(userId)
            .AsNoTracking()
            .Include(x => x.Sets.OrderBy(s => s.ExerciseId).ThenBy(s => s.SetNumber))
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return session ?? throw new NotFoundException($"Workout Session {id} was not found.");
    }

    public async Task<WorkoutSession> CreateAsync(CreateWorkoutSessionRequest request, Guid userId)
    {
        var sets = request.Sets ?? [];

        // The same set number may not appear twice for one exercise
        var duplicate = sets
            .GroupBy(s => (s.ExerciseId, s.SetNumber))
            .FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
            throw new ConflictException(
                $"Set number {duplicate.Key.SetNumber} appears more than once for exercise {duplicate.Key.ExerciseId}.");

        // Every exercise must be visible to the user (own custom exercises or defaults)
        var exerciseIds = sets.Select(s => s.ExerciseId).Distinct().ToList();
        if (exerciseIds.Count > 0)
        {
            var visibleIds = await db.Exercises
                .Where(e => exerciseIds.Contains(e.Id)
                            && (e.CreatedByUserId == userId || e.CreatedByUserId == null))
                .Select(e => e.Id)
                .ToListAsync();

            var missing = exerciseIds.Except(visibleIds).ToList();
            if (missing.Count > 0)
                throw new NotFoundException($"Exercise(s) not found: {string.Join(", ", missing)}.");
        }

        var workoutSession = new WorkoutSession
        {
            UserId = userId,
            Date = request.Date,
            Notes = request.Notes,
            // EF sets SessionId on each set automatically when saving the session
            Sets = sets.Select(s => new SetEntry
            {
                ExerciseId = s.ExerciseId,
                SetNumber = s.SetNumber,
                Weight = s.Weight,
                Reps = s.Reps,
                Rpe = s.Rpe,
                IsWarmup = s.IsWarmup
            }).ToList()
        };

        db.WorkoutSessions.Add(workoutSession);
        // One SaveChanges = one transaction: the session and all its sets are saved together or not at all
        await db.SaveChangesAsync();
        return workoutSession;
    }

    public async Task<WorkoutSession> UpdateAsync(int id, UpdateWorkoutSessionRequest request, Guid userId)
    {
        // OwnedBy makes sure users can only update their own sessions
        // Include sets so the returned session is complete
        var workoutSession = await OwnedBy(userId).Include(x => x.Sets).FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Workout Session {id} was not found.");

        workoutSession.Date = request.Date;
        workoutSession.Notes = request.Notes;

        await db.SaveChangesAsync();
        return workoutSession;
    }

    public async Task DeleteAsync(int id, Guid userId)
    {
        var workoutSession = await OwnedBy(userId).FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Workout Session {id} was not found.");

        // The sets of this session are removed too (cascade delete)
        db.WorkoutSessions.Remove(workoutSession);
        await db.SaveChangesAsync();
    }
}
