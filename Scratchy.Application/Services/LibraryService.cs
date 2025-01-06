using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libRepo;
        public LibraryService(ILibraryRepository libRepo) {
            _libRepo = libRepo;
        }
        public async Task<bool> AddAsync(LibraryEntry libEntry)
        {
           await _libRepo.AddAsync(libEntry);
            return true;
        }

        public Task<Library> GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
