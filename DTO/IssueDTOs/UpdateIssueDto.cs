using System.ComponentModel.DataAnnotations;
using SimpleTaskTracker.Models;

namespace SimpleTaskTracker.DTO;

public class UpdateIssueDto
{
    public string? Title { get; set; } = null!;
    public string? Description { get; set; }
}
