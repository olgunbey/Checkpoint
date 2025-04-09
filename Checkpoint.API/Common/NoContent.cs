using System.Text.Json.Serialization;

namespace Checkpoint.API.Common
{
    public class NoContent
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public string Errors { get; set; }
    }
}
