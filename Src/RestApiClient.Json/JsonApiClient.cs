using RestApiClient.Json.Serialization;
using System.Net.Http;

namespace RestApiClient.Json
{
    public class JsonApiClient : Core.RestApiClient
    {
        public JsonApiClient(HttpClient httpClient) : base(httpClient, new JsonSerializer())
        {
        }

        public JsonApiClient(HttpMessageHandler httpMessageHandler)
            : base(new HttpClient(httpMessageHandler), new JsonSerializer())
        {
        }

        public JsonApiClient(HttpClientHandler httpClientHandler)
            : base(new HttpClient(httpClientHandler), new JsonSerializer())
        {
        }

        public JsonApiClient() : this(new HttpClient())
        {

        }

        public Newtonsoft.Json.JsonSerializer JsonSerializer => ((JsonSerializer)_serializer).Serializer;
    }
}
