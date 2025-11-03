using SimpleTaskTracker.Enums;

namespace SimpleTaskTracker.DTO;

public class IssueRelationListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public RelationType RelationType { get; set; }
}
