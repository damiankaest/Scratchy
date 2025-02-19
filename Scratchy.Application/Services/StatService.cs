using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class StatService : IStatService
    {
        public StatService()
        {
            
        }
        public async Task<UserStatisticDto> GetUserStatsByListOfScratches(List<Scratch> listOfScratches)
        {
            var userStatistic = new UserStatisticDto(listOfScratches);
            return userStatistic;
        }
    }
}
