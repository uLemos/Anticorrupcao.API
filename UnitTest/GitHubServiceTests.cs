using Anticorrupção.API.Models;
using Anticorrupção.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace UnitTest
{
    public class GitHubServiceTests
    {
        private readonly IGitHubService _gitHubService;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public GitHubServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            var services = new ServiceCollection();
            services.AddHttpClient<IGitHubService, GitHubService>();

            var serviceProvider = services.BuildServiceProvider();
            _gitHubService = serviceProvider.GetRequiredService<IGitHubService>();
        }

        [Fact]
        public async Task CreateRepository_ValidRequest_ReturnsRepositoryId()
        {
            // Arrange
            var request = new CreateRepositoryRequest
            {
                Name = "my-repo",
                Description = "My repository",
                Private = false
            };

            var repositoryId = 12345;
            var responseContent = new { id = repositoryId };
            var responseJson = JsonSerializer.Serialize(responseContent);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _gitHubService.CreateRepository(request);

            // Assert
            Assert.Equal(repositoryId.ToString(), result.ToString());
        }
    }
}
