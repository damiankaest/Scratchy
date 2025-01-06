using Scratchy.Domain.DB;
using Scratchy.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IUserDataRepository : IRepository<User>
    {
        Task<List<User>> GetMediaDataByQuery(string query);
    }
}
