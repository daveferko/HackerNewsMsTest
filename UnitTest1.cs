using Moq;
using Moq.Protected;
using nextech_HackerNews.Services;
using System.Net;

namespace HackerNews
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<HttpMessageHandler>? _mockHttpMessageHandler;
        private HttpClient? _httpClient;
        private HackerNewService? _hackerNewService; // Assuming HackerNewsApi is the class containing GetNewStoryDetailsAsync method

        [TestInitialize]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _hackerNewService = new HackerNewService(_httpClient); // Assuming HackerNewsApi constructor accepts HttpClient
        }

        [TestMethod]
        public async Task GetNewStoryDetailsAsync_StoryListIDs()
        {

            // Arrange
            var newStoriesURL = "https://hacker-news.firebaseio.com/v0/newstories.json";
            var newStoriesJson = "[41605302]"; // Simulate a valid JSON response with story IDs

            // Create a mock HttpMessageHandler to return a fake response
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.AbsoluteUri == newStoriesURL
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(newStoriesJson)
                });

            // Create HttpClient using the mocked HttpMessageHandler
            var httpClient = new HttpClient(handlerMock.Object);

            // Create an instance of the service, passing the mocked HttpClient
            var service = new HackerNewService(httpClient);

            // Act
            var result = await service.GetNewStoriesIdsAsync(newStoriesURL);

            // Assert
            Assert.IsNotNull(result, "Expected a non-null result");

        }
    }
}

