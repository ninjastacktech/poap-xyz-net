using Newtonsoft.Json;

namespace Poap;

public class Paginated
{
    [JsonProperty("total")]
    public long? Total { get; set; }

    [JsonProperty("offset")]
    public long? Offset { get; set; }

    [JsonProperty("limit")]
    public int? Limit { get; set; }
}

public class PaginatedItems<T> : Paginated
{
    [JsonProperty("items")]
    public List<T>? Items { get; set; }
}

public class PaginatedTokens<T> : Paginated
{
    [JsonProperty("tokens")]
    public List<T>? Tokens { get; set; }
}
