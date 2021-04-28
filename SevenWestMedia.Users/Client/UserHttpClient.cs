using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly.CircuitBreaker;
using SevenWestMediaTechInterview.Client.Dto;
using JsonException = System.Text.Json.JsonException;

namespace SevenWestMediaTechInterview.Client
{
    /// <summary>
    /// Provides access to an external user store via an HttpClient.
    /// </summary>
    public class UserHttpClient : IUserHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserHttpClient> _logger;

        public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetConnectionString("UserClientUrl"));
            _logger = logger;
            
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
                var response = await _httpClient.GetAsync("sampletest");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(content);
            }
            catch (BrokenCircuitException bce)
            {
                _logger.LogError(bce, "Circuit breaker invoked connecting to external service endpoint at {endpoint}.", _httpClient.BaseAddress);
            }
            catch (JsonException je)
            {
                _logger.LogError(je, "Error parsing JSON response from endpoint.");
            }
            catch (JsonReaderException jre)
            {
                _logger.LogError(jre, "Error parsing JSON response from endpoint.");
            }
            catch (JsonSerializationException jse)
            {
                _logger.LogError(jse, "Error parsing JSON response from endpoint.");
            }
            catch (HttpRequestException hre)
            {
                _logger.LogError(hre, "Error connecting to external service endpoint at {endpoint}.", _httpClient.BaseAddress);
            }

            return users;
        }
    }
}
