using Microsoft.EntityFrameworkCore;

using SimpleTaskTracker.Services.Interfaces;
using SimpleTaskTracker.Data;
using SimpleTaskTracker.Models;
using SimpleTaskTracker.DTO;
using SimpleTaskTracker.Exceptions;
using SimpleTaskTracker.Enums;

namespace SimpleTaskTracker.Services;

public class IssueService : IIssueService
{
    private readonly SimpleTaskTrackerDbContext _context;

    public IssueService(SimpleTaskTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<List<IssueListDto>> GetAllAsync()
    {
        var issues = await _context.IssueItems.Select(i => new IssueListDto
        {
            Id = i.Id,
            Title = i.Title,
            CreatedAt = i.CreatedAt
        }).ToListAsync();

        return issues;
    }

    public async Task<FullIssueDto?> BrowseIssueAsync(int id)
    {
        IssueItem? issue = await _context.IssueItems
            .Include(i => i.RelationsFrom)
                .ThenInclude(r => r.ToIssue)
            .Include(i => i.RelationsTo)
                .ThenInclude(r => r.FromIssue)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue is null)
            throw new IdNotFoundInDatabase($"Issue with id {id} not found.");

        FullIssueDto fullIssueDTO = new FullIssueDto(issue);
        return fullIssueDTO;
    }

    public async Task<FullIssueDto> CreateIssueAsync(CreateIssueDto createIssueDto)
    {
        var issue = new IssueItem
        {
            Title = createIssueDto.Title,
            Description = createIssueDto.Description
        };

        _context.IssueItems.Add(issue);
        await _context.SaveChangesAsync();

        var fullIssueDTO = new FullIssueDto(issue);
        return fullIssueDTO;
    }

    public async Task DeleteIssueAsync(int id)
    {
        var issue = await _context.IssueItems
        .Include(i => i.RelationsFrom)
        .Include(i => i.RelationsTo)
        .FirstOrDefaultAsync(i => i.Id == id);

        if (issue is null)
            throw new IdNotFoundInDatabase($"Issue with id {id} not found.");

        _context.IssueRelations.RemoveRange(issue.RelationsFrom);
        _context.IssueRelations.RemoveRange(issue.RelationsTo);

        _context.IssueItems.Remove(issue);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateIssueAsync(int id, UpdateIssueDto updateIssueDto)
    {
        // для кучи полей, есть вариант с AutoMapper
        var issue = await _context.IssueItems.FindAsync(id);

        if (issue is null)
            throw new IdNotFoundInDatabase($"Issue with id {id} not found.");

        issue.Title = updateIssueDto.Title ?? issue.Title;
        issue.Description = updateIssueDto.Description ?? issue.Description;
        issue.UpdatedAT = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<FullIssueDto> CloneIssueAsync(int id)
    {
        var issue = await _context.IssueItems.FindAsync(id);

        if (issue is null)
            throw new IdNotFoundInDatabase($"Issue with id {id} not found.");

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

        var fullIssueDTO = new FullIssueDto(clonedIssue);

        return fullIssueDTO;
    }

}
