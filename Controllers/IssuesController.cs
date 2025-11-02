using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Data;
using SimpleTaskTracker.DTO;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly SimpleTaskTrackerDbContext _context;

    public IssuesController(SimpleTaskTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Getall()
    {
        var issues = await _context.IssueItems.Select(i => new IssueListDto
        {
            Id = i.Id,
            Title = i.Title
        }).ToListAsync();

        return Ok(issues);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> BrowseIssue(int id)
    {
        var issue = await _context.IssueItems
            .Where(i => i.Id == id)
            .Select(i => new FullIssueDTO
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAT,
                RelationsFrom = i.RelationsFrom
                    .Select(r => new IssueListDto
                    {
                        Id = r.FromIssue.Id,
                        Title = r.FromIssue.Title
                    }).ToList(),
                RelationsTo = i.RelationsTo
                    .Select(r => new IssueListDto
                    {
                        Id = r.ToIssue.Id,
                        Title = r.FromIssue.Title
                    }
                    ).ToList()
            }).FirstOrDefaultAsync();

        if (issue is null)
            return NotFound();

        return Ok(issue);
    }
}