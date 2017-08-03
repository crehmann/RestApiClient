using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestApiClient.Json.Serialization
{
    public class JsonContent : HttpContent
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly object _value;

        public JsonContent(object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            _serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
            _value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                using (var writer = new StreamWriter(gzip))
                {
                    _serializer.Serialize(writer, _value);
                }
            });
        }
    }
}
