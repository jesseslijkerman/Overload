namespace Overload.Domain.Entities;

public class WorkoutSession
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public List<SetEntry> Sets { get; set; } = [];
}
