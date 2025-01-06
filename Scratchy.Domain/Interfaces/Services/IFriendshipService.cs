using Scratchy.Domain.DTO.Request;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task<bool> AcceptFriendRequestAsync(AcceptFriendRequestDto acceptFriendRequestDto);
    }
}
