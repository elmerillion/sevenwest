using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SevenWestMediaTechInterview.Data;
using SevenWestMediaTechInterview.Models;

namespace SevenWestMediaTechInterview
{
    /// <summary>
    /// Service layer access to query and manipulate Users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="id">The users id.</param>
        /// <returns>The specified user, or null if no user exists.</returns>
        public async Task<User> GetUser(int id)
        {
            var user = await _userRepository.GetUser(id);

            return user;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _userRepository.GetUsers();

            return users;
        }

        /// <summary>
        /// Gets all users with the specified predicate.
        /// </summary>
        /// <param name="predicate">Predicate to filter the selection via a LINQ Where clause.</param>
        /// <returns>A list of all users filtered by the supplied predicate.</returns>
        public async Task<IEnumerable<User>> GetUsers(Expression<Func<User, bool>> predicate)
        {
            var users = await this.GetUsers();

            if (predicate != null)
            {
                users = users.Where(predicate.Compile());
            }

            return users;
        }

        /// <summary>
        /// Gets genders and gender counts by age.
        /// </summary>
        /// <returns>A sorted dictionary of ages with the genders and counts per age.</returns>
        /// <remarks>
        /// We assume the range of ages is between 0 (for newborns) and 130. The operation will run in what is effectively constant time given the
        /// use of an array corresponding to the ages, and the minimal sort time of the SortedDictionary implementation with very few genders to sort.
        /// </remarks>
        public async Task<SortedDictionary<int, IDictionary<string, int>>> GetUserGenderCountsByAge()
        {
            var users = await GetUsers();
            var ageToGenders = new IDictionary<string, int>[130];

            foreach (var user in users)
            {
                // A users age corresponds to their array index. Update the relevant index to include the additional gender.
                var ageToGenderMap = ageToGenders[user.Age];
                if (ageToGenderMap != null)
                {
                    if (ageToGenderMap.ContainsKey(user.Gender))
                    {
                        ageToGenderMap[user.Gender] = ageToGenderMap[user.Gender] + 1;
                    }
                    else
                    {
                        ageToGenderMap[user.Gender] = 1;
                    }

                    ageToGenders[user.Age] = ageToGenderMap;
                }
                else
                {
                    var genderMapping = new Dictionary<string, int>();
                    genderMapping[user.Gender] = 1;
                    ageToGenders[user.Age] = genderMapping;
                }
            }

            // Trim from 130 entries to only include actual results. We're guaranteed ordering by using the SortedDictionary.
            var result = new SortedDictionary<int, IDictionary<string, int>>();
            for (int i = 0; i < ageToGenders.Length; i++)
            {
                if (ageToGenders[i] != null)
                {
                    result.Add(i, ageToGenders[i]);
                }
            }

            return result;
        }
    }
}