using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SevenWestMediaTechInterview.Client;
using SevenWestMediaTechInterview.Client.Dto;

namespace SevenWestMediaTechInterview.UnitTests
{
    public class UserHttpClientTests
    {
        [Test]
        public async Task GetUsers_ValidEndpointResponse_Succeeds()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockUserServiceResponse = JsonSerializer.Serialize(
                new List<User>
                {
                    new User() {Id = 1, Age = 21, Gender = "M", GivenName = "First", Surname = "Last"}
                });

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Content = new StringContent(mockUserServiceResponse);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });
            var client = new HttpClient(mockHttpMessageHandler.Object);
            httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(client);
            var logger = new Mock<ILogger<UserHttpClient>>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection(It.IsAny<string>())[It.IsAny<string>()]).Returns("http://null");
            
            var userHttpClient = new UserHttpClient(client, logger.Object, configuration.Object);

            var users = await userHttpClient.GetUsers();

            Assert.IsNotEmpty(users);
        }

        [Test]
        public async Task GetUsers_InvalidJsonResponse_EmptyResult()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockUserServiceResponse = JsonSerializer.Serialize("[]");

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Content = new StringContent(mockUserServiceResponse);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });
            var client = new HttpClient(mockHttpMessageHandler.Object);
            httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(client);
            var logger = new Mock<ILogger<UserHttpClient>>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection(It.IsAny<string>())[It.IsAny<string>()]).Returns("http://null");

            var userHttpClient = new UserHttpClient(client, logger.Object, configuration.Object);

            var users = await userHttpClient.GetUsers();

            Assert.IsEmpty(users);
        }

        [Test]
        public async Task GetUsers_InvalidEndpointResponse_EmptyResult()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    return response;
                });
            var client = new HttpClient(mockHttpMessageHandler.Object);
            httpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(client);
            var logger = new Mock<ILogger<UserHttpClient>>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection(It.IsAny<string>())[It.IsAny<string>()]).Returns("http://null");

            var userHttpClient = new UserHttpClient(client, logger.Object, configuration.Object);

            var users = await userHttpClient.GetUsers();

            Assert.IsEmpty(users);
        }
    }
}