
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IMongoRepository<PostDocument>
    {
        public Task<List<PostDocument>> GetAllByUserIdAsync(string userId);
    }
}
