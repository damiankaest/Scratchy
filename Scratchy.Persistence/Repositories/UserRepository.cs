using Microsoft.EntityFrameworkCore;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Persistence.DB;

namespace Scratchy.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ScratchItDbContext _context;

        public UserRepository(ScratchItDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetByQueryAsync(string query, int limit)
        {
            return await _context.Users
                .Where(u => EF.Functions.Like(u.Username, $"{query}%"))
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByFirebaseIdAsync(string id)
        {
            return await _context.Users.Where(x=>x.FirebaseId == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<User>> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(int userId, int currentUserId)
        {
            var user = await _context.Users
                // Lade alle Scratches des Users und das dazugehörige Album (falls vorhanden)
                .Include(u => u.Scratches)
                    .ThenInclude(s => s.Album)
                // Lade die Followers und Followings, um deren Anzahl zu bestimmen
                .Include(u => u.Followers)
                .Include(u => u.Followings)
                // Falls du ShowCases (oder weitere Navigationen) laden möchtest, kannst du hier weitere Include-Methoden anhängen.
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return null;
            }
            bool isFollowing = user.Followers.Any(f => f.FollowerId == currentUserId);

            var recentScratches = user.Scratches?
                .OrderByDescending(s => s.CreatedAt)
                .Take(10)
                .Select(s => new RecentScratches
                {
                    ScratchId = s.ScratchId,
                    // Hier wird angenommen, dass deine Album-Entität die Eigenschaften AlbumName und AlbumImageUrl besitzt.
                    AlbumName = s.Album != null ? s.Album.Title : string.Empty,
                    AlbumImageUrl = s.Album != null ? s.Album.CoverImageUrl: string.Empty,
                    Rating = s.Rating,
                    CreatedAt = s.CreatedAt
                })
                .ToList();

            // Optional: Wenn du den aktuell angemeldeten User (currentUserId) kennst, kannst du prüfen,
            // ob dieser dem angefragten User folgt. Beispiel:
            //bool isFollowing = user.Followers.Any(f => f.FollowerId == currentUserId);

            var userProfileDto = new UserProfileDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                UserImageUrl = user.ProfilePictureUrl,
                ScratchCount = user.Scratches?.Count ?? 0,
                FollowersCount = user.Followers?.Count ?? 0,
                FollowingCount = user.Followings?.Count ?? 0,
                RecentScratches = recentScratches,
                IsFollowing = isFollowing // oder z. B. isFollowing, wenn du den aktuellen Benutzer berücksichtigst.
            };

            return userProfileDto;
        }
    }
}
