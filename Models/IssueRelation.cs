namespace SimpleTaskTracker.Models;

public enum RelationType
{
    Block,
    Clone,
    Depend
}

public class IssueRelation
{
    public int Id { get; set; }
    public int FromIssueId { get; set; }
    public IssueItem FromIssue { get; set; } = null!;
    public int ToIssueId { get; set; }
    public IssueItem ToIssue { get; set; } = null!;

    public RelationType RelationType{ get; set; }
}