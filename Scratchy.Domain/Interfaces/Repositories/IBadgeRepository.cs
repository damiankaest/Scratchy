using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IBadgeRepository : IMongoRepository<BadgeDocument>
    {
        ///// <summary>
        ///// Retrieves a single Badge by its unique identifier.
        ///// </summary>
        ///// <param name="id">The unique identifier of the Badge.</param>
        ///// <returns>The Badge if found; otherwise null.</returns>
        //Task<BadgeDocument> GetByIdAsync(int id);

        ///// <summary>
        ///// Retrieves all Badges.
        ///// </summary>
        ///// <returns>A list of all Badges.</returns>
        //Task<IEnumerable<BadgeDocument>> GetAllAsync();

        ///// <summary>
        ///// Adds a new Badge to the data store.
        ///// </summary>
        ///// <param name="badge">The Badge entity to create.</param>
        //Task AddAsync(BadgeDocument badge);

        ///// <summary>
        ///// Updates an existing Badge.
        ///// </summary>
        ///// <param name="badge">The Badge entity with updated information.</param>
        //Task UpdateAsync(BadgeDocument badge);

        ///// <summary>
        ///// Deletes a Badge by its unique identifier.
        ///// </summary>
        ///// <param name="id">The unique identifier of the Badge to delete.</param>
        //Task DeleteAsync(string id);

        ///// <summary>
        ///// Checks if a Badge with the given identifier exists.
        ///// </summary>
        ///// <param name="id">The unique identifier of the Badge.</param>
        ///// <returns>True if the Badge exists; otherwise false.</returns>
        //Task<bool> ExistsAsync(string id);

    }
}
