using SimpleTaskTracker.Models;

namespace SimpleTaskTracker.DTO;

public class FullIssueDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<IssueRelationListDto> RelationsFrom { get; set; } = new List<IssueRelationListDto>();
    public List<IssueRelationListDto> RelationsTo { get; set; } = new List<IssueRelationListDto>();

    public FullIssueDto()
    {
        RelationsFrom = new List<IssueRelationListDto>();
        RelationsTo = new List<IssueRelationListDto>();
    }

    public FullIssueDto(IssueItem issue)
    {
        this.Id = issue.Id;
        this.Title = issue.Title;
        this.Description = issue.Description;
        this.CreatedAt = issue.CreatedAt;
        this.UpdatedAt = issue.UpdatedAT;

        RelationsFrom = issue.RelationsFrom?
            .Select(r => new IssueRelationListDto {
                Id = r.ToIssue.Id,
                Title = r.ToIssue.Title,
                RelationType = r.RelationType
            })
            .ToList() ?? new List<IssueRelationListDto>();

        RelationsTo = issue.RelationsTo?
            .Select(r => new IssueRelationListDto {
                Id = r.FromIssue.Id,
                Title = r.FromIssue.Title,
                RelationType = r.RelationType
            })
            .ToList() ?? new List<IssueRelationListDto>();
    }
}
