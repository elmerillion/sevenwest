using System.Collections.Generic;
using System.Threading.Tasks;
using SevenWestMediaTechInterview.Client.Dto;

namespace SevenWestMediaTechInterview.Client
{
    /// <summary>
    /// Provides access to an external user store via an HttpClient.
    /// </summary>
    public interface IUserHttpClient
    {
        /// <summary>
        /// Get users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public Task<IEnumerable<User>> GetUsers();
    }
}
