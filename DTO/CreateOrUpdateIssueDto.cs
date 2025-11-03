using System.ComponentModel.DataAnnotations;

namespace SimpleTaskTracker.DTO;

public class CreateOrUpdateIssueDto
{
    [Required]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}
