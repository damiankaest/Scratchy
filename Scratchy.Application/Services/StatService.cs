using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Application.Services
{
    public class StatService : IStatService
    {
        public StatService()
        {
            
        }
        public async Task<UserStatisticDto> GetUserStatsByListOfScratches(List<ScratchDocument> listOfScratches)
        {
            var userStatistic = new UserStatisticDto(listOfScratches);
            return userStatistic;
        }
    }
}
