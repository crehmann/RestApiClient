using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiClient.Core.UnitTests.TestHelpers
{
    internal class MessageHandlerMock : HttpMessageHandler
    {
        private Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsyncFunction =
            (request, token) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted));

        public HttpRequestMessage LatestRequestMessage { get; private set; }

        public void OnSendAsync(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> fn)
        {
            _sendAsyncFunction = fn ?? throw new ArgumentNullException(nameof(fn));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LatestRequestMessage = request;
            return _sendAsyncFunction(request, cancellationToken);
        }
    }
}