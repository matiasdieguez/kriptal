using System;
using Newtonsoft.Json;

namespace Kriptal.Models
{
    public partial class StampResultData
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
