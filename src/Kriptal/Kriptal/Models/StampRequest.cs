using Newtonsoft.Json;

namespace Kriptal.Models
{
    public partial class StampRequest
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
