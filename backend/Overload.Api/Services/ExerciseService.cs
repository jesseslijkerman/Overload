using Microsoft.EntityFrameworkCore;
using Overload.Api.Data;
using Overload.Api.Exceptions;
using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public class ExerciseService(OverloadDbContext db) : IExerciseService
{
    // Default exercises have CreatedByUserId == null and are visible to everyone
    private IQueryable<Exercise> VisibleTo(Guid userId) =>
        db.Exercises.Where(x => x.CreatedByUserId == userId || x.CreatedByUserId == null);

    // Only a user's own exercises may be updated or deleted
    private IQueryable<Exercise> OwnedBy(Guid userId) =>
        db.Exercises.Where(x => x.CreatedByUserId == userId);

    public async Task<List<Exercise>> GetAllAsync(Guid userId)
    {
        return await VisibleTo(userId)
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Exercise> GetByIdAsync(int id, Guid userId)
    {
        var exercise = await VisibleTo(userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return exercise ?? throw new NotFoundException($"Exercise {id} was not found.");
    }

    public async Task<Exercise> CreateAsync(CreateExerciseRequest request, Guid userId)
    {
        var name = request.Name.Trim();
        await EnsureNameIsFreeAsync(name, userId, excludeId: null);

        var exercise = new Exercise
        {
            Name = name,
            Category = NormalizeCategory(request.Category),
            IsCustom = true,
            CreatedByUserId = userId
        };

        db.Exercises.Add(exercise);
        await db.SaveChangesAsync();
        return exercise;
    }

    public async Task<Exercise> UpdateAsync(int id, UpdateExerciseRequest request, Guid userId)
    {
        var exercise = await OwnedBy(userId).FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Exercise {id} was not found.");

        var name = request.Name.Trim();
        await EnsureNameIsFreeAsync(name, userId, excludeId: id);

        exercise.Name = name;
        exercise.Category = NormalizeCategory(request.Category);

        await db.SaveChangesAsync();
        return exercise;
    }

    public async Task DeleteAsync(int id, Guid userId)
    {
        var exercise = await OwnedBy(userId).FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Exercise {id} was not found.");

        // The foreign key is set to cascade: without this check, deleting an exercise
        // would also delete every set that was logged with it.
        var isUsed = await db.SetEntries.AnyAsync(s => s.ExerciseId == id);
        if (isUsed)
            throw new ConflictException("This exercise is used in logged sets and cannot be deleted.");

        db.Exercises.Remove(exercise);
        await db.SaveChangesAsync();
    }

    private async Task EnsureNameIsFreeAsync(string name, Guid userId, int? excludeId)
    {
        var lowered = name.ToLower();
        var exists = await VisibleTo(userId)
            .AnyAsync(x => x.Name.ToLower() == lowered && (excludeId == null || x.Id != excludeId));

        if (exists)
            throw new ConflictException($"An exercise named '{name}' already exists.");
    }

    private static string? NormalizeCategory(string? category) =>
        string.IsNullOrWhiteSpace(category) ? null : category.Trim();
}
