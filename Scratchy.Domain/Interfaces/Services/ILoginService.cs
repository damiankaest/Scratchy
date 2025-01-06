using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.Request;

namespace Scratchy.Domain.Interfaces.Services
{
    public interface ILoginService
    {
        Task<string> LoginAsync(LoginRequestDTO loginRequest);
        Task<string> RegisterAsync(CreateUserRequestDto newUser);
    }
}
