using Moq;
using RestApiClient.Core.Request;
using RestApiClient.Core.Response;
using RestApiClient.Core.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace RestApiClient.Core.UnitTests.Response
{
    public class ApiResponseTests
    {
        [Fact]
        public void ApiResponse_ShouldThrowException_WhenParameterNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ApiResponse(null, new HttpResponseMessage()));
            Assert.Throws<ArgumentNullException>(() => new ApiResponse(new Mock<ISerializer>().Object, null));
            Assert.Throws<ArgumentNullException>(() => new ApiRequest(string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => ApiRequest.To(null));

            Assert.Throws<ArgumentNullException>(() => new ApiRequest<String>(null, string.Empty));
            Assert.Throws<ArgumentNullException>(() => new ApiRequest<String>(string.Empty, string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => ApiRequest<String>.To(null, string.Empty));
        }

        [Fact]
        public void ApiResponse_ShouldInitializeProperties_WhenHttpResponseMessageOk()
        {
            // arrange
            var expectedResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            // act
            var response = new ApiResponse(new Mock<ISerializer>().Object, expectedResponseMessage);
            response.EnsureSuccessStatusCode();

            // assert
            Assert.Equal(expectedResponseMessage, response.Message);
            Assert.Equal(expectedResponseMessage.Headers, response.Headers);
            Assert.Equal(expectedResponseMessage.IsSuccessStatusCode, response.IsSuccessStatusCode);
            Assert.Equal(expectedResponseMessage.StatusCode, response.StatusCode);
        }

        [Fact]
        public void EnsureSuccessStatusCode_ShouldThrowException_WhenHttpResponseMessageOk()
        {
            // arrange
            var expectedResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            // act
            var response = new ApiResponse(new Mock<ISerializer>().Object, expectedResponseMessage);

            Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode());
        }

        [Fact]
        public async Task ParseAsync_DeserializesContentWithHelpOfSerializer()
        {
            // arrange
            const string expectedContent = "Test";
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(expectedContent)
            };
            var serializer = new Mock<ISerializer>();
            serializer
                .Setup(x => x.DeserializeAsync<string>(It.Is<Stream>(s => expectedContent.Equals(new StreamReader(s).ReadToEnd()))))
                .Returns(Task.FromResult(expectedContent));

            // act
            var response = new ApiResponse(serializer.Object, responseMessage);
            var responseContent = await response.ParseAsync<string>().ConfigureAwait(false);

            // assert
            Assert.Equal(expectedContent, responseContent);
        }
    }
}
