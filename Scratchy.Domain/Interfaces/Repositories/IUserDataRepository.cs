
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserDataRepository : IMongoRepository<UserDocument>
    {
        Task<List<User>> GetMediaDataByQuery(string query);
    }
}
