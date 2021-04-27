using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using SevenWestMediaTechInterview.Client.Dto;

namespace SevenWestMediaTechInterview.Client
{
    /// <summary>
    /// Provides access to an external user store via an HttpClient.
    /// </summary>
    public class UserHttpClient : IUserHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserHttpClient> _logger;
        private readonly IConfiguration _configuration;

        public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Get users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <remarks>This method will return an empty list if it fails to retrieve data successfully to allow quiet continuation.</remarks>
        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> users = new List<User>();

            try
            {
                users = await _httpClient.GetFromJsonAsync<IEnumerable<User>>(new Uri($"{_configuration.GetConnectionString("UserClientUrl")}/sampletest"));
            }
            catch (BrokenCircuitException bce)
            {
                _logger.LogError(bce, "Circuit breaker invoked connecting to external service endpoint at {endpoint}.", _httpClient.BaseAddress);
            }
            catch (JsonException je)
            {
                _logger.LogError(je, "Error parsing JSON response from endpoint.");
            }
            catch (HttpRequestException hre)
            {
                _logger.LogError(hre, "Error connecting to external service endpoint at {endpoint}.", _httpClient.BaseAddress);
            }

            return users;
        }
    }
}
