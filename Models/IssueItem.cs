namespace SimpleTaskTracker.Models;

public class IssueItem
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAT { get; set; }

    public List<IssueRelation> RelationsFrom { get; set; } = new List<IssueRelation>();
    public List<IssueRelation> RelationsTo { get; set; } = new List<IssueRelation>();

    public IssueItem ShallowCopy()
    {
        return new IssueItem {
            Title = this.Title,
            Description = this.Description
        };
    }
}
