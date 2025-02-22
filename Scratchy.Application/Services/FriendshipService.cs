using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private IFriendshipRepository _friendshipRepository;
        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;   
        }
        public async Task<bool> AcceptFriendRequestAsync(AcceptFriendRequestDto acceptFriendRequestDto)
        {
            if (acceptFriendRequestDto == null) return false;
            var userFriendIds = await _friendshipRepository.GetByIdAsync(acceptFriendRequestDto.RequestId);

            return true;
        }
    }
}
