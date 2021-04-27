using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SevenWestMediaTechInterview.Client;
using SevenWestMediaTechInterview.Models;

namespace SevenWestMediaTechInterview.Data
{
    /// <summary>
    /// Provides access to users in the data store.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IUserHttpClient _userHttpClient;
        private readonly IMapper _mapper;

        public UserRepository(IUserHttpClient userHttpClient, IMapper mapper)
        {
            _userHttpClient = userHttpClient;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="id">The users id.</param>
        /// <returns>The specified user, or null if no user exists.</returns>
        public async Task<User> GetUser(int id)
        {
            var users = await _userHttpClient.GetUsers();
            var user = users.FirstOrDefault(u => u.Id == id);
            var mappedUser = _mapper.Map<User>(user);

            return mappedUser;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> result = new List<User>();

            var users = await _userHttpClient.GetUsers();
            result = _mapper.Map<IEnumerable<User>>(users);

            return result;
        }
    }
}
