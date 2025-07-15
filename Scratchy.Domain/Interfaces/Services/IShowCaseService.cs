using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IShowCaseService
    {
        Task<bool> CreateNewShowCaseAsync(CreateShowCaseRequestDto createDto, string userId);
        Task<ShowCaseResponseDTO> GetAllShowCasesFromUserByIdAsync(string userId);
        Task<bool> UpdateShowcaseAsync(UpdateShowCaseDto updateDto);
    }
}
