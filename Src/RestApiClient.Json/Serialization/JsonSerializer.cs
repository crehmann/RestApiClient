using Newtonsoft.Json;
using RestApiClient.Core.Serialization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApiClient.Json.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
        }

        public JsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            Serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
        }

        public Newtonsoft.Json.JsonSerializer Serializer { get; } = new Newtonsoft.Json.JsonSerializer();

        public Task<TContent> DeserializeAsync<TContent>(Stream stream)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var reader = new StreamReader(stream))
                using (var json = new JsonTextReader(reader))
                {
                    return Serializer.Deserialize<TContent>(json);
                }
            });
        }

        public HttpContent Serialize(object data) => new JsonContent(data, Serializer);
    }
}
