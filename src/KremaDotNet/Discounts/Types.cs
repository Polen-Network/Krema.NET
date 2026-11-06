using System.Text.Json.Serialization;
using KremaDotNet.Checkout;
using KremaDotNet.Common;

namespace KremaDotNet.Discounts;

[JsonConverter(typeof(JsonStringEnumConverter<DiscountEntityStatus>))]
public enum DiscountEntityStatus
{
    [JsonStringEnumMemberName("deleted")] Deleted,
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("draft")] Draft,
    [JsonStringEnumMemberName("expired")] Expired,
    [JsonStringEnumMemberName("scheduled")] Scheduled,
}

[JsonConverter(typeof(JsonStringEnumConverter<DiscountEntityType>))]
public enum DiscountEntityType
{
    [JsonStringEnumMemberName("percentage")] Percentage,
    [JsonStringEnumMemberName("fixed")] Fixed,
}

[JsonConverter(typeof(JsonStringEnumConverter<CouponDurationType>))]
public enum CouponDurationType
{
    [JsonStringEnumMemberName("forever")] Forever,
    [JsonStringEnumMemberName("once")] Once,
    [JsonStringEnumMemberName("repeating")] Repeating,
}

/// <summary>
/// A Creem discount.
/// </summary>
public class DiscountEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("status")]
    public required DiscountEntityStatus Status { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("type")]
    public required DiscountEntityType Type { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; set; }

    [JsonPropertyName("expiry_date")]
    public DateTimeOffset? ExpiryDate { get; set; }

    [JsonPropertyName("max_redemptions")]
    public int? MaxRedemptions { get; set; }

    [JsonPropertyName("duration")]
    public CouponDurationType? Duration { get; set; }

    [JsonPropertyName("duration_in_months")]
    public int? DurationInMonths { get; set; }

    [JsonPropertyName("applies_to_products")]
    public List<string>? AppliesToProducts { get; set; }

    [JsonPropertyName("redeem_count")]
    public int? RedeemCount { get; set; }
}

/// <summary>
/// Paginated discount list.
/// </summary>
public class DiscountListEntity
{
    [JsonPropertyName("items")]
    public required List<DiscountEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/discounts</c>.
/// </summary>
public class CreateDiscountRequestEntity
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("type")]
    public required DiscountEntityType Type { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("percentage")]
    public decimal? Percentage { get; set; }

    [JsonPropertyName("expiry_date")]
    public DateTimeOffset? ExpiryDate { get; set; }

    [JsonPropertyName("max_redemptions")]
    public int? MaxRedemptions { get; set; }

    [JsonPropertyName("duration")]
    public required CouponDurationType Duration { get; set; }

    [JsonPropertyName("duration_in_months")]
    public int? DurationInMonths { get; set; }

    [JsonPropertyName("applies_to_products")]
    public required List<string> AppliesToProducts { get; set; }
}
