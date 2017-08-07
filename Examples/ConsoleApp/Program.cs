using ConsoleApp.Service;
using RestApiClient.Json;
using System;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal static class Program
    {
        private static GitUserInfoService _service;

        private static void Main()
        {
            InitializeService();

            while (true)
            {
                var username = Console.ReadLine();
                Request(username).GetAwaiter().GetResult();
            }
        }

        private static void InitializeService()
        {
            var apiClient = new JsonApiClient();
            apiClient.HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyAgent/1.0");
            _service = new GitUserInfoService(apiClient);
        }

        private static async Task Request(string username)
        {
            (await _service.GetUserInfoAsync(username).ConfigureAwait(false))
            .OnSuccess(x => Console.WriteLine(x.ToString()))
            .OnFailure(x => Console.WriteLine(x));
        }
    }
}