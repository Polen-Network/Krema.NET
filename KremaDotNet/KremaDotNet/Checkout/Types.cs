using System.Text.Json;
using System.Text.Json.Serialization;

public class ProductBody
{
    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    [JsonPropertyName("discount_code")]
    public string? DiscountCode { get; set; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }
}

public class CustomField
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("label")]
    public required string Label { get; set; }

    [JsonPropertyName("optional")]
    public bool? Optional { get; set; }

    [JsonPropertyName("text")]
    public TextOptions? Text { get; set; }
}

public class TextOptions
{
    [JsonPropertyName("max_length")]
    public int MaxLength { get; set; }

    [JsonPropertyName("min_length")]
    public int MinLength { get; set; }
}

public class Order
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required string Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("customer")]
    public Dictionary<string, JsonElement>? Customer { get; set; }

    [JsonPropertyName("product")]
    public Dictionary<string, JsonElement>? Product { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("fx_amount")]
    public decimal FxAmount { get; set; }

    [JsonPropertyName("fx_currency")]
    public required string FxCurrency { get; set; }

    [JsonPropertyName("fx_rate")]
    public decimal FxRate { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("affiliate")]
    public string? Affiliate { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required string UpdatedAt { get; set; }
}

public class CheckoutResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required string Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("request_id")]
    public required string RequestId { get; set; }

    [JsonPropertyName("product")]
    public Dictionary<string, JsonElement>? Product { get; set; }

    [JsonPropertyName("units")]
    public int Units { get; set; }

    [JsonPropertyName("order")]
    public Order? Order { get; set; }

    [JsonPropertyName("subscription")]
    public Dictionary<string, JsonElement>? Subscription { get; set; }

    [JsonPropertyName("customer")]
    public Dictionary<string, JsonElement>? Customer { get; set; }

    [JsonPropertyName("custom_fields")]
    public List<CustomField> CustomFields { get; set; } = [];

    [JsonPropertyName("checkout_url")]
    public required string CheckoutUrl { get; set; }

    [JsonPropertyName("success_url")]
    public string? SuccessUrl { get; set; }

    [JsonPropertyName("metadata")]
    public List<Dictionary<string, JsonElement>> Metadata { get; set; } = [];
}