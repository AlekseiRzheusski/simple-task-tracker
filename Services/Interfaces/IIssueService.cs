using SimpleTaskTracker.DTO;
namespace SimpleTaskTracker.Services.Interfaces;
public interface IIssueService
{
    Task<List<IssueListDto>> GetAllAsync();
    Task<FullIssueDto?> BrowseIssueAsync(int id);
    Task<FullIssueDto> CreateIssueAsync(CreateIssueDto createIssueDto);
    Task DeleteIssueAsync(int id);
    Task UpdateIssueAsync(int id, UpdateIssueDto updateIssueDto);
    Task<FullIssueDto> CloneIssueAsync(int id);
}