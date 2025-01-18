using Scratchy.Domain.Interfaces.Repositories;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ILibraryService
    {
        Task<bool> AddAsync(LibraryEntry libEntry);
    }
}
