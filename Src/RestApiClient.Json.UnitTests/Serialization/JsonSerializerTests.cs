using RestApiClient.Json.Serialization;
using System;
using Xunit;

namespace RestApiClient.Json.UnitTests.Serialization
{
    public class JsonSerializerTests
    {
        [Fact]
        public void JsonSerializer_ThrowsArgumentNullException_WhenNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonSerializer(null));
        }
    }
}
