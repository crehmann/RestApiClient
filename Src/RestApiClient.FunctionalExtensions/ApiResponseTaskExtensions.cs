using CSharpFunctionalExtensions;
using RestApiClient.Core.Exceptions;
using RestApiClient.Core.Response;
using System;
using System.Threading.Tasks;

namespace RestApiClient.FunctionalExtensions
{
    public static class ApiResponseTaskExtensions
    {
        public static async Task<Result<T>> ParseResultAsync<T>(this ApiResponse response)
        {
            try
            {
                return Result.Ok(await response.ParseAsync<T>().ConfigureAwait(false));
            }
            catch (DeserializationException ex)
            {
                return await Task.FromResult(Result.Fail<T>(ex.Message)).ConfigureAwait(false);
            }
        }

        public static async Task<Result<TModel>> ParseAndMapResultAsync<TDto, TModel>(this ApiResponse response, Func<TDto, TModel> map)
        {
            try
            {
                var dto = await response.ParseAsync<TDto>().ConfigureAwait(false);
                var model = map(dto);
                return Result.Ok(model);
            }
            catch (DeserializationException ex)
            {
                return await Task.FromResult(Result.Fail<TModel>(ex.Message)).ConfigureAwait(false);
            }
        }

        public static async Task<Result<TModel>> ParseAndMapResultAsync<TDto, TModel>(this ApiResponse response, Func<TDto, TModel> map,
            Func<DeserializationException, Result<TModel>> onDeserializationError)
        {
            try
            {
                var dto = await response.ParseAsync<TDto>().ConfigureAwait(false);
                var model = map(dto);
                return Result.Ok(model);
            }
            catch (DeserializationException ex)
            {
                return onDeserializationError(ex);
            }
        }
    }
}
