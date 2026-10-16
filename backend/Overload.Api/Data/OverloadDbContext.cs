// Overload.Api/Data/OverloadDbContext.cs
using Microsoft.EntityFrameworkCore;
using Overload.Domain.Entities;

namespace Overload.Api.Data;

public class OverloadDbContext(DbContextOptions<OverloadDbContext> options) : DbContext(options)
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<SetEntry> SetEntries => Set<SetEntry>();
}