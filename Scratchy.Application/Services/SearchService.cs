using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

public class SearchService : ISearchService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IUserRepository _userRepository;

    public SearchService(
        IAlbumRepository albumRepository,
        IArtistRepository artistRepository,
        IUserRepository userRepository)
    {
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Album>> SearchAlbumsAsync(string query)
    {
        return await _albumRepository.GetByQueryAsync(query, 10);
    }

    public async Task<IEnumerable<Artist>> SearchArtistsAsync(string query)
    {
        return await _artistRepository.GetByQueryAsync(query, 10);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string query)
    {
        return await _userRepository.GetByQueryAsync(query, 10);
    }

    public async Task<(IEnumerable<Album> Albums, IEnumerable<Artist> Artists, IEnumerable<User> Users)> SearchAllAsync(string query)
    {
        var albumTask = _albumRepository.GetByQueryAsync(query, 5);
        var artistTask = _artistRepository.GetByQueryAsync(query, 5);
        var userTask = _userRepository.GetByQueryAsync(query, 5);

        await Task.WhenAll(albumTask, artistTask, userTask);

        return (await albumTask, await artistTask, await userTask);
    }
}