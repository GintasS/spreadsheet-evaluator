using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Services;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Services
{
    public class HubServiceTests
    {
        private readonly IHubService _hubService;
        private const string TestContent = "test content";

        public HubServiceTests()
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(TestContent)
                });

            var applicationSettings = new ApplicationSettings
            {
                HubApiUrlGetJobs = @"http://anyurl"
            };

            var applicationSettingsMock = new Mock<IOptionsMonitor<ApplicationSettings>>();
            applicationSettingsMock.Setup(c => c.CurrentValue).Returns(applicationSettings);

            _hubService = new HubService(applicationSettingsMock.Object, new HttpClient(mockMessageHandler.Object));
        }

        [Fact]
        public void Should_Send_Get_Request_Successfully()
        {
            // Act
            var httpResponse = _hubService.GetJobs();
            var contentString = httpResponse.Content.ReadAsStringAsync().Result;

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            contentString.Should().Be(TestContent);
        }

        [Fact]
        public void Should_Send_Post_Request_Successfully()
        {
            // Act
            var httpResponse = _hubService.PostJobs(@"http://anyurl", "test");
            var contentString = httpResponse.Content.ReadAsStringAsync().Result;

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            contentString.Should().Be(TestContent);
        }
    }
}
