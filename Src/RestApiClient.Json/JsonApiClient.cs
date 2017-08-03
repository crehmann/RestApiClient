using RestApiClient.Core.Diagnostic;
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
            : base(new HttpClient(new LoggingHandler(httpMessageHandler)), new JsonSerializer())
        {
        }

        public JsonApiClient() : this(new HttpClient(new LoggingHandler(new HttpClientHandler())))
        {

        }

        public Newtonsoft.Json.JsonSerializer JsonSerializer => ((JsonSerializer)_serializer).Serializer;
    }
}
