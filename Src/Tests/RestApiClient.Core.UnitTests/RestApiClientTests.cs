using Moq;
using RestApiClient.Core.Request;
using RestApiClient.Core.Serialization;
using RestApiClient.Core.UnitTests.TestHelpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RestApiClient.Core.UnitTests
{
    public class RestApiClientTests
    {
        private readonly Mock<ISerializer> _serializer = new Mock<ISerializer>();
        private readonly MessageHandlerMock _messageHandler = new MessageHandlerMock();
        private readonly IRestApiClient _restApiClient;

        public RestApiClientTests()
        {
            _restApiClient = new RestApiClient(new HttpClient(_messageHandler), _serializer.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsSuccessfulResult_WhenStatusCodeOk()
        {
            // arrange
            _messageHandler.OnSendAsync((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            // act
            var result = await _restApiClient.GetAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            // assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpMethod.Get, _messageHandler.LatestRequestMessage.Method);
        }

        [Fact]
        public async Task GetAsync_ReturnsRequestError_OnHttpRequestException()
        {
            // arrange
            _restApiClient.HttpClient.Timeout = TimeSpan.FromMilliseconds(100);
            _messageHandler.OnSendAsync((request, token) => throw new HttpRequestException());

            // act
            var result = await _restApiClient.GetAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            // assert
            Assert.True(result.IsRequestError);
        }

        [Fact]
        public async Task GetAsync_ReturnsServerError_WhenStatusError()
        {
            // arrange
            _messageHandler.OnSendAsync((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)));

            // act
            var result = await _restApiClient.GetAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            // assert
            Assert.True(result.IsStatusCodeError);
            Assert.Equal(HttpMethod.Get, _messageHandler.LatestRequestMessage.Method);
        }

        [Fact]
        public async Task PostAsync_IsUsingHttpPost()
        {
            // arrange
            _messageHandler.OnSendAsync((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            // act
            var result = await _restApiClient.PostAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            Assert.Equal(HttpMethod.Post, _messageHandler.LatestRequestMessage.Method);
        }

        [Fact]
        public async Task PutAsync_IsUsingHttpPut()
        {
            // arrange
            _messageHandler.OnSendAsync((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            // act
            var result = await _restApiClient.PutAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            Assert.Equal(HttpMethod.Put, _messageHandler.LatestRequestMessage.Method);
        }

        [Fact]
        public async Task DeleteAsync_IsUsingHttpDelete()
        {
            // arrange
            _messageHandler.OnSendAsync((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            // act
            var result = await _restApiClient.DeleteAsync(ApiRequest.To("https://someurl.com")).ConfigureAwait(false);

            Assert.Equal(HttpMethod.Delete, _messageHandler.LatestRequestMessage.Method);
        }
    }
}