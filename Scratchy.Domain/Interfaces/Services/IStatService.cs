using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IStatService
    {
        Task<UserStatisticDto> GetUserStatsByListOfScratches(List<ScratchDocument> listOfScratches);
    }
}
