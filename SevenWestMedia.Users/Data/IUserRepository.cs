using System.Collections.Generic;
using System.Threading.Tasks;
using SevenWestMediaTechInterview.Models;

namespace SevenWestMediaTechInterview.Data
{
    /// <summary>
    /// Provides access to users in the data store.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="id">The users id.</param>
        /// <returns>The specified user, or null if no user exists.</returns>
        public Task<User> GetUser(int id);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public Task<IEnumerable<User>> GetUsers();
    }
}