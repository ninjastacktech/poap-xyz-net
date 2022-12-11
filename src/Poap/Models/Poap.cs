using Newtonsoft.Json;

namespace Poap;

public class Poap
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("created")]
    public DateTime? Created { get; set; }

    [JsonProperty("owner")]
    public Owner? Owner { get; set; }

    [JsonProperty("transferCount")]
    public string? TransferCount { get; set; }
}
