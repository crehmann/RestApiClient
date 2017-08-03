using RestApiClient.Core.Request;
using System;
using System.Threading;
using Xunit;

namespace RestApiClient.Core.UnitTests.Request
{
    public class ApiRequestTests
    {
        [Fact]
        public void ApiRequest_ShouldThrowException_WhenParameterNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ApiRequest(null));
            Assert.Throws<ArgumentNullException>(() => new ApiRequest(string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => ApiRequest.To(null));

            Assert.Throws<ArgumentNullException>(() => new ApiRequest<String>(null, string.Empty));
            Assert.Throws<ArgumentNullException>(() => new ApiRequest<String>(string.Empty, string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => ApiRequest<String>.To(null, string.Empty));
        }

        [Fact]
        public void ApiRequest_ShouldInitializeProperties()
        {
            // arrange
            const string expectedUrl = "https://someurl.com";
            const string expectedContent = "Content";
            var expectedCancellationTokenSource = new CancellationTokenSource();

            // act
            var request = new ApiRequest(expectedUrl, expectedCancellationTokenSource);
            var requestWithContent = new ApiRequest<string>(expectedUrl, expectedContent);

            // assert
            Assert.Equal(expectedUrl, request.Url);
            Assert.Equal(expectedUrl, requestWithContent.Url);
            Assert.Equal(expectedCancellationTokenSource.Token, request.CancellationToken);
            Assert.Equal(expectedCancellationTokenSource.IsCancellationRequested, request.IsCancellationRequested);
            Assert.Equal(expectedContent, requestWithContent.Content);
        }
    }
}
