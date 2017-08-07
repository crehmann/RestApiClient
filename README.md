# RestApiClient

> Note: this library is under development. An early alpha can be found on [nuget](https://www.nuget.org/packages/RestApiClient.Json/)

RestApiClient is a thin wrapper around the .net HttpClient and makes accessing your API a breeze. It takes away the necessary boilerplate code to implement a fast and efficient communication with your API endpoints.

## Features
* JSON de/serialization using JSON.NET
* GZIP compression for requests
* Function extensions method to use it together with [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions)

## Examples
A console application example can be found in the `Examples/ConsoleApp` folder.

### Creating a JsonApiClient
You can either inject your own HttpClient instance to the JsonApiClient or just invoke the constructor without any parameters.

```csharp
var apiClient = new JsonApiClient();
```

The JsonApiClient exposes the underlying HttpClient via a property. This way you can easily add your own default header values:
```csharp
apiClient.HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyAgent/1.0");
```

### Using the JsonApiClient
You can either send your own custom `HttpRequestMessage` or use one of the four shortcuts for `GET`, `PUT`, `POST` `DELETE` requests. 
```csharp
Task<ApiResult> SendAsync(ApiRequest<HttpRequestMessage> request);
Task<ApiResult> DeleteAsync(ApiRequest request);
Task<ApiResult> GetAsync(ApiRequest request);
Task<ApiResult> PostAsync(ApiRequest request);
Task<ApiResult> PutAsync(ApiRequest request);
```

Example:
```csharp
var result = await apiClient.GetAsync(ApiRequest.To($"https://api.github.com/users/{username}"));
if (result.IsSuccess)
{
    using (var response = result.Response)
    {
        return await response.ParseAsync<UserInfo>();
    }
}
```

### Using the JsonApiClient in a more functional way
The [RestApiClient.FunctionalExtensions](https://www.nuget.org/packages/RestApiClient.FunctionalExtensions/) provides extensions method to use the RestApiClient togther with the [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) in a more functional way:

```csharp
public Task<Result<UserInfo>> GetUserInfoAsync(string username) => ApiRequest.To($"https://api.github.com/users/{username}")
        .UsingMethod(apiClient.GetAsync)
        .SelectResultAsync(
            onSuccess: HandleUserInfoResponse,
            onRequestError: HandleRequestError<UserInfo>,
            onStatusCodeError: HandleStatusCodeError<UserInfo>);

// Helper function to map the DTO from the response to a model
private static Task<Result<UserInfo>> HandleUserInfoResponse(ApiResult result)
    => Disposable.UsingAsync(
        () => result.Response,
        x => x.ParseAndMapResultAsync<UserInfoDto, UserInfo>(UserInfoDto.MapToModel));

// Here you can implement your reusable error handling
private static Task<Result<T>> HandleRequestError<T>(ApiResult result)
    => Task.FromResult(Result.Fail<T>(result.Exception.Message));

private static Task<Result<T>> HandleStatusCodeError<T>(ApiResult result)
    => Task.FromResult(Result.Fail<T>($"Status Code Error: {result.Response.StatusCode}"));
```
