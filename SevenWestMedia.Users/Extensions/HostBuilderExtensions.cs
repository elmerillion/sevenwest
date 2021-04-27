using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using SevenWestMediaTechInterview.Client;
using SevenWestMediaTechInterview.Data;

namespace SevenWestMediaTechInterview.Extensions
{
    /// <summary>
    /// Configuration extensions for defining policy and dependency injection.
    /// </summary>
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureUserServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddScoped<IUserService, UserService>()
                    .AddScoped<IUserRepository, UserRepository>();
                services.AddHttpClient<IUserHttpClient, UserHttpClient>(client =>
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    })
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());
            });

            return hostBuilder;
        }

        /// <summary>
        /// Gets the Polly retry policy definition.
        /// </summary>
        /// <returns>The retry policy.</returns>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Gets the Polly circuit breaker policy definition.
        /// </summary>
        /// <returns>The circuit breaker policy.</returns>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(1));
        }


    }
}
