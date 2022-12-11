using Newtonsoft.Json;

namespace Poap;

public class Token
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("tokenId")]
    public string? TokenId { get; set; }

    [JsonProperty("owner")]
    public string? Owner { get; set; }

    [JsonProperty("layer")]
    public string? Layer { get; set; }

    [JsonProperty("created")]
    public DateTime? Created { get; set; }

    [JsonProperty("event")]
    public Event? Event { get; set; }

    [JsonProperty("supply")]
    public Supply? Supply { get; set; }
}
