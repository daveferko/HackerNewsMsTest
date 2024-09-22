using Moq;
using Moq.Protected;
using nextech_HackerNews.Services;
using System.Net;

namespace HackerNews
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async Task GetNewStoryDetailsAsync_StoryListDetails()
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
            var result = await service.GetNewStoriesAsync(newStoriesURL);

            // Assert
            Assert.IsNotNull(result, "Expected a non-null result");
            //Assert.AreEqual(3, result.Length, "Expected 3 story IDs");
            //Assert.AreEqual(1, result[0], "Expected the first story ID to be 1");
            //Assert.AreEqual(2, result[1], "Expected the second story ID to be 2");
            //Assert.AreEqual(3, result[2], "Expected the third story ID to be 3");
        }
    }
}