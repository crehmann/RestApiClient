using CSharpFunctionalExtensions;
using RestApiClient.Core.Response;
using System;
using System.Threading.Tasks;

namespace RestApiClient.FunctionalExtensions
{
    public static class ApiResultTaskExtensions
    {
        public static async Task<Result> SelectResultAsync(
            this Task<ApiResult> result,
            Func<ApiResult, Task<Result>> onSuccess,
            Func<ApiResult, Task<Result>> onError,
            Func<ApiResult, Task<Result>> onCancellation)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return await onSuccess(apiResult).ConfigureAwait(false);
            if (apiResult.IsRequestError) return await onError(apiResult).ConfigureAwait(false);
            return await onCancellation(apiResult).ConfigureAwait(false);
        }

        public static async Task<Result> SelectResultAsync(
            this Task<ApiResult> result,
            Func<ApiResult, Task<Result>> onSuccess,
            Func<ApiResult, Task<Result>> onError,
            Func<ApiResult, Result> onCancellation)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return await onSuccess(apiResult).ConfigureAwait(false);
            if (apiResult.IsRequestError) return await onError(apiResult).ConfigureAwait(false);
            return onCancellation(apiResult);
        }

        public static async Task<Result> SelectResultAsync(
            this Task<ApiResult> result,
            Func<ApiResult, Task<Result>> onSuccess,
            Func<ApiResult, Result> onError,
            Func<ApiResult, Result> onCancellation)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return await onSuccess(apiResult).ConfigureAwait(false);
            if (apiResult.IsRequestError) return onError(apiResult);
            return onCancellation(apiResult);
        }

        public static async Task<Result> SelectResultAsync(
            this Task<ApiResult> result,
            Func<ApiResult, Result> onSuccess,
            Func<ApiResult, Task<Result>> onError,
            Func<ApiResult, Result> onCancellation)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return onSuccess(apiResult);
            if (apiResult.IsRequestError) return await onError(apiResult).ConfigureAwait(false);
            return onCancellation(apiResult);
        }

        public static async Task<Result> SelectResultAsync(
            this Task<ApiResult> result,
            Func<ApiResult, Result> onSuccess,
            Func<ApiResult, Result> onError,
            Func<ApiResult, Result> onCancellation)
        {
            var apiResult = await result.ConfigureAwait(false);
            if (apiResult.IsSuccess) return onSuccess(apiResult);
            if (apiResult.IsRequestError) return onError(apiResult);
            return onCancellation(apiResult);
        }
    }
}
