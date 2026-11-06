using System.Text.Json;
using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;
using Krema.NET.Customers;
using Krema.NET.Products;
using Krema.NET.Transactions;

namespace Krema.NET.Subscriptions;

#region Enums

[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionStatus>))]
public enum SubscriptionStatus
{
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("canceled")] Canceled,
    [JsonStringEnumMemberName("unpaid")] Unpaid,
    [JsonStringEnumMemberName("paused")] Paused,
    [JsonStringEnumMemberName("trialing")] Trialing,
    [JsonStringEnumMemberName("scheduled_cancel")] ScheduledCancel,
    [JsonStringEnumMemberName("past_due")] PastDue,
}

[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionCollectionMethod>))]
public enum SubscriptionCollectionMethod
{
    [JsonStringEnumMemberName("charge_automatically")] ChargeAutomatically,
}

[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionUpdateBehavior>))]
public enum SubscriptionUpdateBehavior
{
    [JsonStringEnumMemberName("proration-charge-immediately")] ProrationChargeImmediately,
    [JsonStringEnumMemberName("proration-charge")] ProrationCharge,
    [JsonStringEnumMemberName("proration-none")] ProrationNone,
}

[JsonConverter(typeof(JsonStringEnumConverter<CancelSubscriptionMode>))]
public enum CancelSubscriptionMode
{
    [JsonStringEnumMemberName("immediate")] Immediate,
    [JsonStringEnumMemberName("scheduled")] Scheduled,
}

[JsonConverter(typeof(JsonStringEnumConverter<CancelSubscriptionOnExecute>))]
public enum CancelSubscriptionOnExecute
{
    [JsonStringEnumMemberName("cancel")] Cancel,
    [JsonStringEnumMemberName("pause")] Pause,
}

#endregion

#region Subscription responses

/// <summary>
/// A Creem subscription.
/// </summary>
public class SubscriptionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    /// <summary>
    /// Either a product ID string or an expanded product object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("product")]
    public required JsonElement Product { get; set; }

    /// <summary>
    /// Either a customer ID string or an expanded customer object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("customer")]
    public required JsonElement Customer { get; set; }

    [JsonPropertyName("items")]
    public List<SubscriptionItemEntity>? Items { get; set; }

    [JsonPropertyName("collection_method")]
    public required SubscriptionCollectionMethod CollectionMethod { get; set; }

    [JsonPropertyName("status")]
    public required SubscriptionStatus Status { get; set; }

    [JsonPropertyName("last_transaction_id")]
    public string? LastTransactionId { get; set; }

    [JsonPropertyName("last_transaction")]
    public TransactionEntity? LastTransaction { get; set; }

    [JsonPropertyName("last_transaction_date")]
    public DateTimeOffset? LastTransactionDate { get; set; }

    [JsonPropertyName("next_transaction_date")]
    public DateTimeOffset? NextTransactionDate { get; set; }

    [JsonPropertyName("current_period_start_date")]
    public DateTimeOffset? CurrentPeriodStartDate { get; set; }

    [JsonPropertyName("current_period_end_date")]
    public DateTimeOffset? CurrentPeriodEndDate { get; set; }

    [JsonPropertyName("canceled_at")]
    public DateTimeOffset? CanceledAt { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("discount")]
    public SubscriptionDiscountEntity? Discount { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }
}

/// <summary>
/// A line item of a subscription.
/// </summary>
public class SubscriptionItemEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("price_id")]
    public string? PriceId { get; set; }

    [JsonPropertyName("units")]
    public int? Units { get; set; }
}

/// <summary>
/// Paginated subscription list.
/// </summary>
public class SubscriptionListEntity
{
    [JsonPropertyName("items")]
    public required List<SubscriptionEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// The discount applied to a subscription.
/// </summary>
/// <remarks>
/// Creem returns this object with camelCase keys (<c>discountCode</c>,
/// <c>durationInMonths</c>), unlike the rest of the API.
/// </remarks>
public class SubscriptionDiscountEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("discountCode")]
    public string? DiscountCode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public SubscriptionDiscountType? Type { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("duration")]
    public SubscriptionDiscountDuration? Duration { get; set; }

    [JsonPropertyName("durationInMonths")]
    public int? DurationInMonths { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionDiscountType>))]
public enum SubscriptionDiscountType
{
    [JsonStringEnumMemberName("percentage")] Percentage,
    [JsonStringEnumMemberName("fixed")] Fixed,
}

[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionDiscountDuration>))]
public enum SubscriptionDiscountDuration
{
    [JsonStringEnumMemberName("forever")] Forever,
    [JsonStringEnumMemberName("once")] Once,
    [JsonStringEnumMemberName("repeating")] Repeating,
}

#endregion

#region Subscription requests

/// <summary>
/// Request body for <c>POST /v1/subscriptions/{id}/cancel</c>.
/// </summary>
public class CancelSubscriptionRequestEntity
{
    [JsonPropertyName("mode")]
    public CancelSubscriptionMode? Mode { get; set; }

    [JsonPropertyName("on_execute")]
    public CancelSubscriptionOnExecute? OnExecute { get; set; }
}

/// <summary>
/// Request body for <c>PATCH /v1/subscriptions/{id}</c>.
/// </summary>
public class UpdateSubscriptionRequestEntity
{
    [JsonPropertyName("items")]
    public List<UpsertSubscriptionItemEntity>? Items { get; set; }

    [JsonPropertyName("update_behavior")]
    public SubscriptionUpdateBehavior? UpdateBehavior { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/subscriptions/{id}/upgrade</c>.
/// </summary>
public class UpgradeSubscriptionRequestEntity
{
    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    [JsonPropertyName("update_behavior")]
    public SubscriptionUpdateBehavior? UpdateBehavior { get; set; }
}

/// <summary>
/// A subscription item sent in an update request. Omit <see cref="Id"/> to create an item.
/// </summary>
public class UpsertSubscriptionItemEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("price_id")]
    public string? PriceId { get; set; }

    [JsonPropertyName("units")]
    public int? Units { get; set; }
}

#endregion
