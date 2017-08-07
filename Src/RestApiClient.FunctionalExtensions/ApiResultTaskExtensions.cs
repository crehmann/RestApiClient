using CSharpFunctionalExtensions;
using RestApiClient.Core.Response;
using System;
using System.Threading.Tasks;

namespace RestApiClient.FunctionalExtensions
{
    public static class ApiResultTaskExtensions
    {
        public static async Task<Result> SelectResultAsync(this Task<ApiResult> result,
            Func<ApiResult, Task<Result>> onSuccess,
            Func<ApiResult, Task<Result>> onRequestError,
            Func<ApiResult, Task<Result>> onStatusCodeError)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return await onSuccess(apiResult).ConfigureAwait(false);
            if (apiResult.IsRequestError) return await onRequestError(apiResult).ConfigureAwait(false);
            if (apiResult.IsStatusCodeError) return await onStatusCodeError(apiResult).ConfigureAwait(false);
            throw new InvalidOperationException("Invalid result state");
        }

        public static async Task<Result<T>> SelectResultAsync<T>(this Task<ApiResult> result,
            Func<ApiResult, Task<Result<T>>> onSuccess,
            Func<ApiResult, Task<Result<T>>> onRequestError,
            Func<ApiResult, Task<Result<T>>> onStatusCodeError)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return await onSuccess(apiResult).ConfigureAwait(false);
            if (apiResult.IsRequestError) return await onRequestError(apiResult).ConfigureAwait(false);
            if (apiResult.IsStatusCodeError) return await onStatusCodeError(apiResult).ConfigureAwait(false);
            throw new InvalidOperationException("Invalid result state");
        }
    }
}
