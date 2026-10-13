namespace Overload.Domain.Entities;

public class SetEntry
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public WorkoutSession Session { get; set; } = null!;
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public int SetNumber { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public decimal? Rpe { get; set; }
    public bool IsWarmup { get; set; }
}
