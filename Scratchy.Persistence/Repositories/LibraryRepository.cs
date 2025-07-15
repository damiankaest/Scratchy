using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using MongoDB.Libmongocrypt;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Persistence.Repositories
{
    public class LibraryRepository : MongoRepository<LibraryEntry>, ILibraryRepository
    {
        private readonly ILogger<LibraryRepository> _libraryRepository;
        public LibraryRepository(MongoDbContext context, ILogger<LibraryRepository> logger) : base(context, logger)
        {
            
        }
        public Task<LibraryEntry> AddAsync(LibraryEntry entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryEntry>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LibraryEntry> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryEntry>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(LibraryEntry entity)
        {
            throw new NotImplementedException();
        }
    }
}
