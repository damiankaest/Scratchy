using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> LoginAsync(LoginRequestDTO loginRequest)
        {
            var tokenService = new TokenService("5d2f8a9fcd0e4b8b9e96c9a6f4e2c7d8");
            var token = tokenService.GenerateToken("user.Id");
            return token;
        }

        public async Task<string> RegisterAsync(CreateUserRequestDto createUserDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
                return "Email already used.";

            var newUser = new User
            {
                Username = createUserDto.Email,
                Email = createUserDto.Email
            };

            //var passwordHasher = new PasswordHasher<User>();
            //newUser.Password = passwordHasher.HashPassword(newUser, createUserDto.Password);

            await _userRepository.AddAsync(new User());

            var userAddResult = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (userAddResult == null)
                return "Try login later";

            return userAddResult.UserId.ToString();
        }
    }
}
