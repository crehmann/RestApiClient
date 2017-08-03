using System;
using System.Threading;

namespace RestApiClient.Core.Request
{
    public class ApiRequest
    {
        private readonly CancellationTokenSource _tcs;

        public ApiRequest(string url, CancellationTokenSource tcs)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
            _tcs = tcs ?? throw new ArgumentNullException(nameof(tcs));
        }

        public ApiRequest(string url) : this(url, new CancellationTokenSource())
        {
        }

        public bool IsCancellationRequested => _tcs.IsCancellationRequested;

        public string Url { get; }

        public CancellationToken CancellationToken => _tcs.Token;

        public void Cancel(bool throwOnFirstException)
        {
            _tcs.Cancel(throwOnFirstException);
        }

        public static ApiRequest To(string url)
        {
            return new ApiRequest(url);
        }
    }

    public class ApiRequest<TContent> : ApiRequest
    {
        public ApiRequest(string url, TContent content) : this(url, content, new CancellationTokenSource())
        {
        }

        public ApiRequest(string url, TContent content, CancellationTokenSource tcs) : base(url, tcs)
        {
            Content = content;
        }

        public TContent Content { get; }

        public static ApiRequest<TContent> To(string url, TContent content)
        {
            return new ApiRequest<TContent>(url, content);
        }
    }
}
