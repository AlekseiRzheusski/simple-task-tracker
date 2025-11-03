using System.ComponentModel.DataAnnotations;
using SimpleTaskTracker.Models;

namespace SimpleTaskTracker.DTO;

public class CreateIssueDto
{
    [Required]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}
