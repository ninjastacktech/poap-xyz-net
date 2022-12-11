using Newtonsoft.Json;

namespace Poap;

public class Supply
{
    [JsonProperty("total")]
    public int? Total { get; set; }

    [JsonProperty("order")]
    public int? Order { get; set; }
}
