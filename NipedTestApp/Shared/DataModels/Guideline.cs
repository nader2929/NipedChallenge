using Microsoft.EntityFrameworkCore;

namespace Shared.DataModels;

[PrimaryKey(nameof(Id))]
[Index(nameof(Category))]
public class Guideline
{
    public int Id { get; set; }
    public required string Category { get; set; }
    public required string Optimal { get; set; }
    public required string NeedsAttention { get; set; }
    public required string SeriousIssue { get; set; }
}