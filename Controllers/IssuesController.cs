using Microsoft.AspNetCore.Mvc;

using SimpleTaskTracker.DTO;
using SimpleTaskTracker.Services.Interfaces;
using SimpleTaskTracker.Exceptions;

namespace SimpleTaskTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;

    public IssuesController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _issueService.GetAllAsync();

        return Ok(issues);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BrowseIssue(int id)
    {
        try
        {
            var fullIssueDTO = await _issueService.BrowseIssueAsync(id);
            return Ok(fullIssueDTO);
        }
        catch (IdNotFoundInDatabase ex)
        {
            Response.Headers["X-Message"] = ex.Message;
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDto createIssueDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var fullIssueDTO = await _issueService.CreateIssueAsync(createIssueDto);

        return CreatedAtAction(
            nameof(BrowseIssue),
            new { id = fullIssueDTO.Id },
            fullIssueDTO);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(int id)
    {
        try
        {
            await _issueService.DeleteIssueAsync(id);
            Response.Headers["X-Message"] = "Issue was removed";
            return NoContent();
        }
        catch (IdNotFoundInDatabase ex)
        {
            Response.Headers["X-Message"] = ex.Message;
            return NotFound();
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, [FromBody] UpdateIssueDto updateIssueDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _issueService.UpdateIssueAsync(id, updateIssueDto);
            Response.Headers["X-Message"] = "Issue was updated";
            return NoContent();
        }
        catch(IdNotFoundInDatabase ex)
        {
            Response.Headers["X-Message"] = ex.Message;
            return NotFound();
        }
    }

    [HttpPost("{id}/clone")]
    public async Task<IActionResult> CloneIssue(int id)
    {
        try
        {
            var fullIssueDTO = await _issueService.CloneIssueAsync(id);
            return CreatedAtAction(
                nameof(BrowseIssue),
                new { id = fullIssueDTO.Id },
                fullIssueDTO);
        }
        catch(IdNotFoundInDatabase ex)
        {
            Response.Headers["X-Message"] = ex.Message;
            return NotFound();
        }

    }
}
