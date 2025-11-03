using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Data;
using SimpleTaskTracker.DTO;
using SimpleTaskTracker.Models;

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
    public async Task<IActionResult> GetAll()
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

    [HttpPost]
    public async Task<IActionResult> CreateIssue([FromBody] CreateOrUpdateIssueDto createIssueDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var issue = new IssueItem { Title = createIssueDto.Title, Description = createIssueDto.Description };
        _context.IssueItems.Add(issue);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(BrowseIssue), new { id = issue.Id }, issue);
    }
}
