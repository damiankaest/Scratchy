
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserDataRepository : IRepository<User>
    {
        Task<List<User>> GetMediaDataByQuery(string query);
    }
}
