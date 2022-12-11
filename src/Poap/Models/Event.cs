using Newtonsoft.Json;

namespace Poap;

public class Event
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("fancy_id")]
    public string? FancyId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("event_url")]
    public string? EventUrl { get; set; }

    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("year")]
    public int? Year { get; set; }

    [JsonProperty("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonProperty("end_date")]
    public DateTime? EndDate { get; set; }

    [JsonProperty("expiry_date")]
    public DateTime? ExpiryDate { get; set; }

    [JsonProperty("supply")]
    public int? Supply { get; set; }

    [JsonProperty("from_admin")]
    public bool? IsFromAdmin { get; set; }

    [JsonProperty("virtual_event")]
    public bool? IsVirtual { get; set; }

    [JsonProperty("private_event")]
    public bool? IsPrivate { get; set; }

    [JsonProperty("event_template_id")]
    public long? EventTemplateId { get; set; }
}
