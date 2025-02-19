using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IShowCaseService
    {
        Task<bool> CreateNewShowCaseAsync(CreateShowCaseRequestDto createDto, int userId);
        Task<ShowCaseResponseDTO> GetAllShowCasesFromUserByIdAsync(int userId);
        Task<bool> UpdateShowcaseAsync(UpdateShowCaseDto updateDto);
    }
}
