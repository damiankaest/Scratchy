using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface ILibraryRepository : IRepository<LibraryEntry>
    {
        Task<IEnumerable<LibraryEntry>> GetByUserIdAsync(string userId);
    }
}
