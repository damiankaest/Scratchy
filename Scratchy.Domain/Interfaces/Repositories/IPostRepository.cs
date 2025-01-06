using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<List<Post>> GetAllByUserIdAsync(string userId);
    }
}
