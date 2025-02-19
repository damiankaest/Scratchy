
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IStatService
    {
        Task<UserStatisticDto> GetUserStatsByListOfScratches(List<Scratch> listOfScratches);
    }
}
