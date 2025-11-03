namespace SimpleTaskTracker.DTO;

public class IssueListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}