using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IShowCaseRepository : IRepository<ShowCase>
    {
        Task<IEnumerable<ShowCase>> GetByUserId(int userId);
    }
}
