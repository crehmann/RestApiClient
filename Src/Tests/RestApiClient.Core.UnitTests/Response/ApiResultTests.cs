using Moq;
using RestApiClient.Core.Request;
using RestApiClient.Core.Response;
using RestApiClient.Core.Serialization;
using System;
using System.Net.Http;
using Xunit;

namespace RestApiClient.Core.UnitTests.Response
{
    public class ApiResultTests
    {
        [Fact]
        public void FromError_ThrowsException_WhenArgumentsInvalid()
        {
            Assert.Throws<ArgumentNullException>(() => ApiResult.FromError(null, null));
            Assert.Throws<ArgumentNullException>(() => ApiResult.FromError(null, new Exception()));
            Assert.Throws<ArgumentNullException>(() => ApiResult.FromError(ApiRequest.To(string.Empty), null));
        }

        [Fact]
        public void FromError_ShouldInitializePropertiesCorrect()
        {
            // arrange
            var expectedRequest = ApiRequest.To(string.Empty);
            var expectedException = new HttpRequestException();

            // act
            var result = ApiResult.FromError(expectedRequest, expectedException);

            // assert
            Assert.Equal(expectedRequest, result.Request);
            Assert.Equal(expectedException, result.Exception);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal(ResultState.RequestError, result.State);
            Assert.True(result.IsRequestError);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void FromResponse_ThrowsException_WhenArgumentInvalid()
        {
            Assert.Throws<ArgumentNullException>(() => ApiResult.FromResponse(null, null));
            Assert.Throws<ArgumentNullException>(() => ApiResult.FromResponse(ApiRequest.To(string.Empty), null));
            Assert.Throws<ArgumentNullException>(
                () => ApiResult.FromResponse(null, new ApiResponse(new Mock<ISerializer>().Object, new HttpResponseMessage())));
        }

        [Fact]
        public void FromResponse_ShouldInitializePropertiesCorrect()
        {
            // arrange
            var expectedRequest = ApiRequest.To(string.Empty);
            var serializer = new Mock<ISerializer>();
            var expectedResponse = new ApiResponse(serializer.Object, new HttpResponseMessage(System.Net.HttpStatusCode.OK));

            // act
            var result = ApiResult.FromResponse(expectedRequest, expectedResponse);

            // assert
            Assert.Equal(expectedRequest, result.Request);
            Assert.Equal(expectedResponse, result.Response);
            Assert.Throws<InvalidOperationException>(() => result.Exception);
            Assert.Equal(ResultState.Success, result.State);
            Assert.False(result.IsRequestError);
            Assert.True(result.IsSuccess);
        }
    }
}
