using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SevenWestMediaTechInterview.Models;

namespace SevenWestMediaTechInterview
{
    /// <summary>
    /// Service layer access to query and manipulate Users.
    /// </summary>
    public interface IUserService
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


        /// <summary>
        /// Gets all users with the specified predicate.
        /// </summary>
        /// <param name="predicate">Predicate to filter the selection via a LINQ Where clause.</param>
        /// <returns>A list of all users filtered by the supplied predicate.</returns>
        public Task<IEnumerable<User>> GetUsers(Expression<Func<User, bool>> predicate);

        /// <summary>
        /// Gets genders and gender counts by age.
        /// </summary>
        /// <returns>A sorted dictionary of ages with the genders and counts per age.</returns>
        public Task<SortedDictionary<int, IDictionary<string, int>>> GetUserGenderCountsByAge();

    }
}