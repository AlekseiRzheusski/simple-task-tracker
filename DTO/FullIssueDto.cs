namespace SimpleTaskTracker.DTO;

public class FullIssueDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<IssueListDto> RelationsFrom { get; set; } = new List<IssueListDto>();
    public List<IssueListDto> RelationsTo { get; set; } = new List<IssueListDto>();
}