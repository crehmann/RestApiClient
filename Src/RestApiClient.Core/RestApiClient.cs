using RestApiClient.Core.Request;
using RestApiClient.Core.Response;
using RestApiClient.Core.Serialization;
using RestApiClient.Core.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApiClient.Core
{
    public class RestApiClient : IRestApiClient
    {
        protected readonly ISerializer _serializer;

        public RestApiClient(HttpClient httpClient, ISerializer serializer)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public RestApiClient(ISerializer serializer) : this(new HttpClient(), serializer)
        {
        }

        public HttpClient HttpClient { get; }

        public Task<ApiResult> DeleteAsync(ApiRequest request)
        {
            return TryRequest(request,
                () => HttpClient.DeleteAsync(request.Url, request.CancellationToken));
        }

        public Task<ApiResult> GetAsync(ApiRequest request)
        {
            return TryRequest(request,
                () => HttpClient.GetAsync(request.Url, request.CancellationToken));
        }

        public Task<ApiResult> PostAsync<TContent>(ApiRequest<TContent> request)
        {
            return Disposable
                .Using(
                    () => _serializer.Serialize(request.Content),
                    content => TryRequest(
                        request,
                        () => HttpClient.PostAsync(request.Url, content, request.CancellationToken)));
        }

        public Task<ApiResult> PostAsync(ApiRequest request)
        {
            return TryRequest(request,
                () => HttpClient.PostAsync(request.Url, null, request.CancellationToken));
        }

        public Task<ApiResult> PutAsync<TContent>(ApiRequest<TContent> request)
        {
            return Disposable
                .Using(
                    () => _serializer.Serialize(request.Content),
                    content => TryRequest(
                        request,
                        () => HttpClient.PutAsync(request.Url, content, request.CancellationToken)));
        }

        public Task<ApiResult> PutAsync(ApiRequest request)
        {
            return TryRequest(request,
                () => HttpClient.PutAsync(request.Url, null, request.CancellationToken));
        }

        public Task<ApiResult> SendAsync(ApiRequest<HttpRequestMessage> request)
        {
            return TryRequest(request,
                () => HttpClient.SendAsync(request.Content, request.CancellationToken));
        }

        private async Task<ApiResult> TryRequest(ApiRequest request, Func<Task<HttpResponseMessage>> requestFunction)
        {
            try
            {
                var responseMessage = await requestFunction().ConfigureAwait(false);
                return ApiResult.FromResponse(request, new ApiResponse(_serializer, responseMessage));
            }
            catch (OperationCanceledException)
            {
                return await Task.FromResult(ApiResult.FromRequestCancellation(request)).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                return await Task.FromResult(ApiResult.FromError(request, ex)).ConfigureAwait(false);
            }
        }
    }
}
