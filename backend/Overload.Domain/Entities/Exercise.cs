namespace Overload.Domain.Entities;

public class Exercise
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Category { get; set; }
    public bool IsCustom { get; set; }
    public Guid? CreatedByUserId { get; set; }
}
