using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Domain.Models;

namespace Scratchy.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IScratchService _scratchService;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            
        }

        public async Task<IEnumerable<ExploreUserDto>> GetByQueryAsync(string query, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Die Suchabfrage darf nicht leer sein.", nameof(query));

            var userResponseList = new List<ExploreUserDto>();

            foreach (var user in await _userRepository.FindAsync(query))
            {
                userResponseList.Add(new ExploreUserDto(user));
            }
            return userResponseList;
        }

        public async Task<IEnumerable<UserDocument>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserDocument> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {id} gefunden.");

            return user;
        }

        public async Task<UserDocument> AddAsync(UserDocument user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _userRepository.CreateAsync(user);
        }

        public async Task<UserDocument> UpdateAsync(UserDocument user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _userRepository.GetByFireBaseId(user.FirebaseId);
            if (existingUser == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {user.Id} gefunden.");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userRepository.GetByFireBaseId(id);
            if (user == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {id} gefunden.");

            //await _userRepository.DeleteAsync(id);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendFriendRequest(UserDocument currentUser, UserDocument userResult)
        {
            throw new NotImplementedException();
        }

        public Task<UserDocument> GetUserByFireBaseId(string currentUserID) => _userRepository.GetByFireBaseId(currentUserID);

        public async Task<UserDocument> GetUserProfileByIdAsync(string currentUserId)
        {
            var userProfileDto = await _userRepository.GetByIdAsync(currentUserId);
            return userProfileDto;
            
        }
    }
}
