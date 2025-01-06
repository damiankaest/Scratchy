using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Scratchy.Domain.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<ExploreUserDto>> GetByQueryAsync(string query, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Die Suchabfrage darf nicht leer sein.", nameof(query));

            var userResponseList = new List<ExploreUserDto>();

            foreach (var user in await _userRepository.GetByQueryAsync(query, limit))
            {
                
                userResponseList.Add(new ExploreUserDto(user));
            }
            return userResponseList;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {id} gefunden.");

            return user;
        }

        public async Task<User> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = await _userRepository.GetByIdAsync("");
            if (existingUser == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {user.Id} gefunden.");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Kein Benutzer mit der ID {id} gefunden.");

            //await _userRepository.DeleteAsync(id);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendFriendRequest(User currentUser, User userResult)
        {
            throw new NotImplementedException();
        }
    }
}
