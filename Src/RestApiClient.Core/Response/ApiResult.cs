using RestApiClient.Core.Request;
using System;
using System.Diagnostics;

namespace RestApiClient.Core.Response
{
    public sealed class ApiResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Exception _exception;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ApiResponse _apiResponse;

        private ApiResult()
        {
        }

        private ApiResult(ApiRequest apiRequest, ApiResponse apiResponse)
        {
            _apiResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
            Request = apiRequest ?? throw new ArgumentNullException(nameof(apiRequest));
            State = apiResponse.IsSuccessStatusCode
                ? ResultState.Success
                : ResultState.StatusCodeError;
        }

        private ApiResult(ApiRequest apiRequest, Exception exception)
        {
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));
            Request = apiRequest ?? throw new ArgumentNullException(nameof(apiRequest));
            State = apiRequest.IsCancellationRequested
                ? ResultState.Cancelled
                : ResultState.RequestError;
        }

        private ApiResult(ApiRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            State = ResultState.Cancelled;

            if (request.IsCancellationRequested) return;
            throw new InvalidOperationException("Request was not cancelled. Either response message or error exception required.");
        }

        #region Properties 

        public Exception Exception
        {
            [DebuggerStepThrough]
            get
            {
                if (IsRequestError) return _exception;
                throw new InvalidOperationException("There is no exception for success.");
            }
        }

        public bool IsSuccess => State == ResultState.Success;

        public bool IsRequestError => State == ResultState.RequestError;

        public bool IsStatusCodeError => State == ResultState.StatusCodeError;

        public ApiRequest Request { get; }

        public ApiResponse Response
        {
            [DebuggerStepThrough]
            get
            {
                if (IsRequestError) throw new InvalidOperationException("There is no response if the request failed.");
                return _apiResponse;
            }
        }

        public ResultState State { get; }

        #endregion

        public static ApiResult FromResponse(ApiRequest apiRequest, ApiResponse apiResponse) => new ApiResult(apiRequest, apiResponse);

        public static ApiResult FromError(ApiRequest apiRequest, Exception exception) => new ApiResult(apiRequest, exception);
    }
}