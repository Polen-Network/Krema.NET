using System.Text.Json.Serialization;

namespace Krema.NET.Credits;

[JsonConverter(typeof(JsonStringEnumConverter<CustomerCreditsErrorType>))]
public enum CustomerCreditsErrorType
{
    [JsonStringEnumMemberName("invalid_request_error")] InvalidRequestError,
    [JsonStringEnumMemberName("api_error")] ApiError,
    [JsonStringEnumMemberName("authentication_error")] AuthenticationError,
    [JsonStringEnumMemberName("rate_limit_error")] RateLimitError,
}

/// <summary>
/// Credits granted by a product feature.
/// </summary>
public class CustomerCredits
{
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("unit_label")]
    public string? UnitLabel { get; set; }

    [JsonPropertyName("bucket_name")]
    public string? BucketName { get; set; }
}

/// <summary>
/// A credit grant movement on a customer's ledger.
/// </summary>
public class CustomerCreditsGrantEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("account_id")]
    public required string AccountId { get; set; }

    [JsonPropertyName("bucket_name")]
    public required string BucketName { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("amount_minor_units")]
    public required string AmountMinorUnits { get; set; }

    [JsonPropertyName("balance_after_minor_units")]
    public required string BalanceAfterMinorUnits { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("occurred_at")]
    public required string OccurredAt { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } = "customer_credits_grant";
}

/// <summary>
/// A credit consumption movement on a customer's ledger.
/// </summary>
public class CustomerCreditsConsumptionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("account_id")]
    public required string AccountId { get; set; }

    [JsonPropertyName("bucket_name")]
    public required string BucketName { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("amount_minor_units")]
    public required string AmountMinorUnits { get; set; }

    [JsonPropertyName("balance_after_minor_units")]
    public required string BalanceAfterMinorUnits { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("occurred_at")]
    public required string OccurredAt { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } = "customer_credits_consumption";
}

/// <summary>
/// A credit auto-recharge that was charged to the customer.
/// </summary>
public class CustomerCreditsAutoRechargeEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } = "customer_credits_auto_recharge";

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("bucket_name")]
    public required string BucketName { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("amount_minor_units")]
    public required string AmountMinorUnits { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("charge_reference")]
    public required string ChargeReference { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }
}

/// <summary>
/// A period during which a customer ran out of credits.
/// </summary>
public class CustomerCreditsExhaustionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } = "customer_credits_exhaustion";

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("bucket_name")]
    public required string BucketName { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("shortfall_minor_units")]
    public required string ShortfallMinorUnits { get; set; }

    [JsonPropertyName("occurred_at")]
    public required string OccurredAt { get; set; }
}

/// <summary>
/// Error details returned by the customer credits endpoints.
/// </summary>
public class CustomerCreditsErrorDetailDto
{
    [JsonPropertyName("type")]
    public required CustomerCreditsErrorType Type { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("param")]
    public string? Param { get; set; }

    [JsonPropertyName("request_id")]
    public required string RequestId { get; set; }
}

/// <summary>
/// Error details returned by the usage metering endpoints.
/// </summary>
public class UsageMeteringErrorDetailApiDto
{
    [JsonPropertyName("type")]
    public required CustomerCreditsErrorType Type { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("param")]
    public string? Param { get; set; }

    [JsonPropertyName("details")]
    public Dictionary<string, System.Text.Json.JsonElement>? Details { get; set; }

    [JsonPropertyName("request_id")]
    public required string RequestId { get; set; }
}
