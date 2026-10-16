using Microsoft.EntityFrameworkCore;
using Overload.Api.Data;
using Overload.Api.Models.Requests;
using Overload.Domain.Entities;

namespace Overload.Api.Services;

public class ExerciseService(OverloadDbContext db) : IExerciseService
{
    public async Task<List<Exercise>> GetAllAsync(Guid userId)
    {
        var exercises = await db.Exercises.Where
            (x => x.CreatedByUserId == userId || x.CreatedByUserId == null).ToListAsync();
        return exercises;
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        return await db.Exercises.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    
    // TODO: More fields and checks for non required fields
    public async Task<Exercise> CreateAsync(CreateExerciseRequest request, Guid userId)
    {
        var exercise = new Exercise
        {
            Name = request.Name,
            Category = request.Category
        };
        db.Exercises.Add(exercise);
        await db.SaveChangesAsync();
        return exercise;
    }

    public Task<Exercise?> UpdateAsync(int id, UpdateExerciseRequest request, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id, Guid userId)
    {
        throw new NotImplementedException();
    }
}