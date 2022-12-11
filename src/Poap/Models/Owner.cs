using Newtonsoft.Json;

namespace Poap
{
    public class Owner
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("tokensOwned")]
        public int? TokensOwned { get; set; }

        [JsonProperty("ens")]
        public string? Ens { get; set; }
    }
}
