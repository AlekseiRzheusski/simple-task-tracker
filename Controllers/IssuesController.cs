using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Data;
using SimpleTaskTracker.DTO;
using SimpleTaskTracker.Models;
using SimpleTaskTracker.Enums;

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
            Title = i.Title,
            CreatedAt = i.CreatedAt
        }).ToListAsync();

        return Ok(issues);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BrowseIssue(int id)
    {
        var issue = await _context.IssueItems
            .Include(i => i.RelationsFrom)
                .ThenInclude(r => r.ToIssue)
            .Include(i => i.RelationsTo)
                .ThenInclude(r => r.FromIssue)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue is null)
            return NotFound();
        
        var fullIssueDTO = new FullIssueDTO(issue);

        return Ok(fullIssueDTO);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDto createIssueDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var issue = new IssueItem { Title = createIssueDto.Title, Description = createIssueDto.Description };
        _context.IssueItems.Add(issue);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(BrowseIssue), new { id = issue.Id }, issue);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(int id)
    {
        var issue = await _context.IssueItems.FindAsync(id);
        if (issue is null)
            return NotFound();

        _context.IssueItems.Remove(issue);
        await _context.SaveChangesAsync();
        Response.Headers["X-Message"] = "Issue was removed";
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, [FromBody] UpdateIssueDto updateIssueDto)
    {
        // для кучи полей, есть вариант с AutoMapper
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var issue = await _context.IssueItems.FindAsync(id);

        if (issue is null)
            return NotFound();

        issue.Title = updateIssueDto.Title ?? issue.Title;
        issue.Description = updateIssueDto.Description ?? issue.Description;
        issue.UpdatedAT = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        Response.Headers["X-Message"] = "Issue was updated";
        return NoContent();
    }

    [HttpPost("{id}/clone")]
    public async Task<IActionResult> CloneIssue(int id)
    {
        var issue = await _context.IssueItems.FindAsync(id);

        if (issue is null)
            return NotFound();

        var clonedIssue = issue.ShallowCopy();

        _context.IssueItems.Add(clonedIssue);
        await _context.SaveChangesAsync();

        _context.IssueRelations.Add(new IssueRelation
        {
            FromIssue = issue,
            ToIssue = clonedIssue,
            RelationType = RelationType.Clone
        });
        await _context.SaveChangesAsync();

        // для корректного вывода
        var fullIssueDTO = new FullIssueDTO(clonedIssue);

        return CreatedAtAction(nameof(BrowseIssue), new { id = clonedIssue.Id }, fullIssueDTO);
    }
}
