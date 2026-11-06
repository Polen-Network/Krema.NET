using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;

namespace Krema.NET.Transactions;

[JsonConverter(typeof(JsonStringEnumConverter<TransactionType>))]
public enum TransactionType
{
    [JsonStringEnumMemberName("payment")] Payment,
    [JsonStringEnumMemberName("invoice")] Invoice,
}

[JsonConverter(typeof(JsonStringEnumConverter<TransactionStatus>))]
public enum TransactionStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("paid")] Paid,
    [JsonStringEnumMemberName("refunded")] Refunded,
    [JsonStringEnumMemberName("partialRefund")] PartialRefund,
    [JsonStringEnumMemberName("chargedBack")] ChargedBack,
    [JsonStringEnumMemberName("uncollectible")] Uncollectible,
    [JsonStringEnumMemberName("declined")] Declined,
    [JsonStringEnumMemberName("canceled")] Canceled,
    [JsonStringEnumMemberName("void")] Void,
}

/// <summary>
/// A payment or invoice transaction.
/// </summary>
public class TransactionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("amount_paid")]
    public decimal? AmountPaid { get; set; }

    [JsonPropertyName("discount_amount")]
    public decimal? DiscountAmount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("type")]
    public required TransactionType Type { get; set; }

    [JsonPropertyName("tax_country")]
    public string? TaxCountry { get; set; }

    [JsonPropertyName("tax_amount")]
    public decimal? TaxAmount { get; set; }

    [JsonPropertyName("status")]
    public required TransactionStatus Status { get; set; }

    [JsonPropertyName("refunded_amount")]
    public decimal? RefundedAmount { get; set; }

    [JsonPropertyName("order")]
    public string? Order { get; set; }

    [JsonPropertyName("subscription")]
    public string? Subscription { get; set; }

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("period_start")]
    public long? PeriodStart { get; set; }

    [JsonPropertyName("period_end")]
    public long? PeriodEnd { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

/// <summary>
/// Paginated transaction list.
/// </summary>
public class TransactionListEntity
{
    [JsonPropertyName("items")]
    public required List<TransactionEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}
