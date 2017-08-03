using RestApiClient.Json.Serialization;
using System;
using Xunit;

namespace RestApiClient.Json.UnitTests.Serialization
{
    public class JsonContentTests
    {
        [Fact]
        public void JsonContent_ThrowsArgumentNullException_WhenSerializerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonContent(string.Empty, null));
        }

        [Fact]
        public void JsonContent_SetsCorrectContentTypeHeader()
        {
            // act
            var jsonContent = new JsonContent(string.Empty, new Newtonsoft.Json.JsonSerializer());

            // assert
            Assert.Equal("application/json", jsonContent.Headers.ContentType.MediaType);
        }
    }
}
