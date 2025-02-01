using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IShowCaseService
    {
        Task<ShowCaseResponseDTO> GetByShowCaseIdAsync(int showCaseId);
        Task<ShowCaseResponseDTO> GetAllShowCasesFromUser(int userId);
    }
}
