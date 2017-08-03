using RestApiClient.Core.Request;
using RestApiClient.Core.Response;
using System;
using System.Threading.Tasks;

namespace RestApiClient.FunctionalExtensions
{
    public static class ApiRequestExtensions
    {
        public static Task<ApiResult> UsingMethod(this ApiRequest request, Func<ApiRequest, Task<ApiResult>> fn) => fn(request);
    }
}
