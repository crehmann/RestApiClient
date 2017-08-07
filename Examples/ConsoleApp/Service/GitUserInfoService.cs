using ConsoleApp.Dto;
using ConsoleApp.Model;
using CSharpFunctionalExtensions;
using RestApiClient.Core;
using RestApiClient.Core.Request;
using RestApiClient.Core.Response;
using RestApiClient.Core.Utils;
using RestApiClient.FunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace ConsoleApp.Service
{
    internal class GitUserInfoService
    {
        private readonly IRestApiClient _apiClient;

        public GitUserInfoService(IRestApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public Task<Result<UserInfo>> GetUserInfoAsync(string username) => ApiRequest.To($"https://api.github.com/users/{username}")
                .UsingMethod(_apiClient.GetAsync)
                .SelectResultAsync(
                    onSuccess: HandleUserInfoResponse,
                    onRequestError: HandleRequestError<UserInfo>,
                    onStatusCodeError: HandleStatusCodeError<UserInfo>);

        private static Task<Result<UserInfo>> HandleUserInfoResponse(ApiResult result)
            => Disposable.UsingAsync(
                () => result.Response,
                x => x.ParseAndMapResultAsync<UserInfoDto, UserInfo>(UserInfoDto.MapToModel));

        private static Task<Result<T>> HandleRequestError<T>(ApiResult result)
        {
            return Task.FromResult(Result.Fail<T>(result.Exception.Message));
        }

        private static Task<Result<T>> HandleStatusCodeError<T>(ApiResult result)
        {
            return Task.FromResult(Result.Fail<T>($"Status Code Error: {result.Response.StatusCode}"));
        }
    }
}
