using Newtonsoft.Json;

namespace ConsoleApp.Dto
{
    internal class ErrorDto
    {
        public string Message { get; set; }

        [JsonProperty(PropertyName = "documentation_url")]
        public string DocumentationUrl { get; set; }
    }
}