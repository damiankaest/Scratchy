using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ILibraryService
    {
        Task<Library> GetByUserId(string userId);
        Task<bool> AddAsync(LibraryEntry libEntry);
    }
}
