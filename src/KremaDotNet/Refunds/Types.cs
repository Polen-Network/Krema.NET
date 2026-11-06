using System.Text.Json;
using System.Text.Json.Serialization;
using KremaDotNet.Checkout;
using KremaDotNet.Transactions;

namespace KremaDotNet.Refunds;

[JsonConverter(typeof(JsonStringEnumConverter<RefundReason>))]
public enum RefundReason
{
    [JsonStringEnumMemberName("duplicate")] Duplicate,
    [JsonStringEnumMemberName("fraudulent")] Fraudulent,
    [JsonStringEnumMemberName("requested_by_customer")] RequestedByCustomer,
    [JsonStringEnumMemberName("other")] Other,
}

[JsonConverter(typeof(JsonStringEnumConverter<RefundStatus>))]
public enum RefundStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("requiresAction")] RequiresAction,
    [JsonStringEnumMemberName("succeeded")] Succeeded,
    [JsonStringEnumMemberName("failed")] Failed,
    [JsonStringEnumMemberName("canceled")] Canceled,
}

/// <summary>
/// A refund created for a transaction.
/// </summary>
public class RefundEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("status")]
    public required RefundStatus Status { get; set; }

    [JsonPropertyName("refund_amount")]
    public required decimal RefundAmount { get; set; }

    [JsonPropertyName("refund_currency")]
    public required string RefundCurrency { get; set; }

    [JsonPropertyName("reason")]
    public required RefundReason Reason { get; set; }

    [JsonPropertyName("transaction")]
    public required TransactionEntity Transaction { get; set; }

    /// <summary>
    /// Either a checkout ID string or an expanded checkout object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("checkout")]
    public JsonElement? Checkout { get; set; }

    /// <summary>
    /// Either an order ID string or an expanded order object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("order")]
    public JsonElement? Order { get; set; }

    /// <summary>
    /// Either a subscription ID string or an expanded subscription object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("subscription")]
    public JsonElement? Subscription { get; set; }

    /// <summary>
    /// Either a customer ID string or an expanded customer object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("customer")]
    public JsonElement? Customer { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

/// <summary>
/// Response returned by the refund endpoints.
/// </summary>
public class RefundResponseEntity
{
    [JsonPropertyName("status")]
    public required RefundStatus Status { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/refunds</c>.
/// </summary>
public class CreateRefundRequestEntity
{
    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; set; }
}
