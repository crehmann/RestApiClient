using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApiClient.Core.Serialization
{
    public interface ISerializer
    {
        Task<T> DeserializeAsync<T>(Stream stream);
        HttpContent Serialize(object data);
    }
}
