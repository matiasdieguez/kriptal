using Newtonsoft.Json;

namespace Kriptal.Models
{
    public partial class StampResult
    {
        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("result")]
        public StampResultData Result { get; set; }
    }
}
