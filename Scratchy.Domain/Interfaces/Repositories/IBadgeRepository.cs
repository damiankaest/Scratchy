using Scratchy.Domain.DB;

namespace Scratchy.Domain.Interfaces.Repositories
{
    public interface IBadgeRepository
    {
        /// <summary>
        /// Retrieves a single Badge by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Badge.</param>
        /// <returns>The Badge if found; otherwise null.</returns>
        Task<Badge> GetByIdAsync(string id);

        /// <summary>
        /// Retrieves all Badges.
        /// </summary>
        /// <returns>A list of all Badges.</returns>
        Task<IEnumerable<Badge>> GetAllAsync();

        /// <summary>
        /// Adds a new Badge to the data store.
        /// </summary>
        /// <param name="badge">The Badge entity to create.</param>
        Task AddAsync(Badge badge);

        /// <summary>
        /// Updates an existing Badge.
        /// </summary>
        /// <param name="badge">The Badge entity with updated information.</param>
        Task UpdateAsync(Badge badge);

        /// <summary>
        /// Deletes a Badge by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Badge to delete.</param>
        Task DeleteAsync(string id);

        /// <summary>
        /// Checks if a Badge with the given identifier exists.
        /// </summary>
        /// <param name="id">The unique identifier of the Badge.</param>
        /// <returns>True if the Badge exists; otherwise false.</returns>
        Task<bool> ExistsAsync(string id);

    }
}
