using RestApiClient.Core.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestApiClient.Core.Response
{
    public class ApiResponse : IDisposable
    {
        private readonly ISerializer _serializer;
        private bool _disposed;

        public ApiResponse(ISerializer serializer, HttpResponseMessage httpResponseMessage)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            Message = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
        }

        ~ApiResponse()
        {
            Dispose(false);
        }

        public HttpResponseMessage Message { get; }

        public HttpStatusCode StatusCode => Message.StatusCode;

        public HttpResponseHeaders Headers => Message.Headers;

        public bool IsSuccessStatusCode => Message.IsSuccessStatusCode;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void EnsureSuccessStatusCode()
        {
            Message.EnsureSuccessStatusCode();
        }

        public async Task<TContent> ParseAsync<TContent>()
        {
            using (var stream = await Message.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                return await _serializer.DeserializeAsync<TContent>(stream).ConfigureAwait(false);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (!disposing) return;
            Message.Dispose();
        }
    }
}
