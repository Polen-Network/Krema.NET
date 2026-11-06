using System.Text.Json.Serialization;

public enum DiscountMode
{
    Test,
    Live,
    Sandbox
}

public enum DiscountStatus
{
    Active,
    Draft,
    Expired,
    Scheduled
}

public enum DiscountType
{
    Percentage,
    Fixed
}

public enum DiscountDuration
{
    Forever,
    Once,
    Repeating
}

public class BaseDiscountBody
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("applies_to_products")]
    public List<string>? AppliesToProducts { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("expiry_date")]
    public string? ExpiryDate { get; set; }

    [JsonPropertyName("max_redemptions")]
    public int? MaxRedemptions { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; set; }

    [JsonPropertyName("duration")]
    public required string Duration { get; set; }

    [JsonPropertyName("duration_in_months")]
    public int? DurationInMonths { get; set; }
}

public class DiscountResponse
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required string Mode { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("applies_to_products")]
    public List<string>? AppliesToProducts { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("duration_in_months")]
    public int? DurationInMonths { get; set; }

    [JsonPropertyName("expiry_date")]
    public string? ExpiryDate { get; set; }

    [JsonPropertyName("max_redemptions")]
    public int? MaxRedemptions { get; set; }

    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; set; }
}