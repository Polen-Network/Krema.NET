using System.Text.Json;
using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;
using Krema.NET.Credits;
using Krema.NET.Customers;
using Krema.NET.Disputes;
using Krema.NET.Products;
using Krema.NET.Refunds;
using Krema.NET.Subscriptions;
using Krema.NET.Transactions;

namespace Krema.NET.Webhooks;

#region Enums

[JsonConverter(typeof(JsonStringEnumConverter<WebhookEventType>))]
public enum WebhookEventType
{
    [JsonStringEnumMemberName("checkout.completed")] CheckoutCompleted,
    [JsonStringEnumMemberName("refund.created")] RefundCreated,
    [JsonStringEnumMemberName("dispute.created")] DisputeCreated,
    [JsonStringEnumMemberName("subscription.active")] SubscriptionActive,
    [JsonStringEnumMemberName("subscription.trialing")] SubscriptionTrialing,
    [JsonStringEnumMemberName("subscription.canceled")] SubscriptionCanceled,
    [JsonStringEnumMemberName("subscription.scheduled_cancel")] SubscriptionScheduledCancel,
    [JsonStringEnumMemberName("subscription.paid")] SubscriptionPaid,
    [JsonStringEnumMemberName("subscription.expired")] SubscriptionExpired,
    [JsonStringEnumMemberName("subscription.unpaid")] SubscriptionUnpaid,
    [JsonStringEnumMemberName("subscription.update")] SubscriptionUpdate,
    [JsonStringEnumMemberName("subscription.past_due")] SubscriptionPastDue,
    [JsonStringEnumMemberName("subscription.paused")] SubscriptionPaused,
    [JsonStringEnumMemberName("customer_credits.exhausted")] CustomerCreditsExhausted,
    [JsonStringEnumMemberName("credits.granted")] CreditsGranted,
    [JsonStringEnumMemberName("credits.consumed")] CreditsConsumed,
    [JsonStringEnumMemberName("credits.auto_recharged")] CreditsAutoRecharged,
}

[JsonConverter(typeof(JsonStringEnumConverter<WebhookStatus>))]
public enum WebhookStatus
{
    [JsonStringEnumMemberName("enabled")] Enabled,
    [JsonStringEnumMemberName("disabled")] Disabled,
}

[JsonConverter(typeof(JsonStringEnumConverter<WebhookDeliveryMode>))]
public enum WebhookDeliveryMode
{
    [JsonStringEnumMemberName("http")] Http,
    [JsonStringEnumMemberName("cli")] Cli,
}

#endregion

#region Webhook endpoints

/// <summary>
/// A configured webhook endpoint.
/// </summary>
public class WebhookEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("store_id")]
    public required string StoreId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("delivery_mode")]
    public required WebhookDeliveryMode DeliveryMode { get; set; }

    [JsonPropertyName("status")]
    public required WebhookStatus Status { get; set; }

    [JsonPropertyName("events")]
    public required List<WebhookEventType> Events { get; set; }

    [JsonPropertyName("secret")]
    public string? Secret { get; set; }
}

/// <summary>
/// Paginated webhook list.
/// </summary>
public class WebhookListEntity
{
    [JsonPropertyName("items")]
    public required List<WebhookEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// A webhook signing secret.
/// </summary>
public class WebhookSecretEntity
{
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/webhooks</c>.
/// </summary>
public class CreateWebhookRequestEntity
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("delivery_mode")]
    public WebhookDeliveryMode? DeliveryMode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("events")]
    public List<WebhookEventType>? Events { get; set; }
}

/// <summary>
/// Request body for <c>PATCH /v1/webhooks/{id}</c>.
/// </summary>
public class UpdateWebhookRequestEntity
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public WebhookStatus? Status { get; set; }

    [JsonPropertyName("events")]
    public List<WebhookEventType>? Events { get; set; }
}

#endregion

#region Pending events

/// <summary>
/// An event waiting in the pending-events feed (CLI delivery mode).
/// </summary>
public class WebhookPendingEventEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("event_type")]
    public required WebhookEventType EventType { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("body")]
    public required string Body { get; set; }

    [JsonPropertyName("headers")]
    public required Dictionary<string, string> Headers { get; set; }
}

/// <summary>
/// Pending webhook events.
/// </summary>
public class WebhookPendingEventListEntity
{
    [JsonPropertyName("items")]
    public required List<WebhookPendingEventEntity> Items { get; set; }
}

/// <summary>
/// Request body for acknowledging a webhook event.
/// </summary>
public class AcknowledgeWebhookEventRequestEntity
{
    [JsonPropertyName("status_code")]
    public required int StatusCode { get; set; }

    [JsonPropertyName("response_body")]
    public string? ResponseBody { get; set; }
}

/// <summary>
/// Response returned after acknowledging a webhook event.
/// </summary>
public class WebhookEventAckEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("success")]
    public required bool Success { get; set; }
}

#endregion

#region Webhook events

/// <summary>
/// Base type for webhook payloads. The concrete type is selected with the
/// <c>eventType</c> discriminator.
/// </summary>
[JsonConverter(typeof(WebhookEventJsonConverter))]
public abstract class WebhookEventEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

public class WebhookCheckoutCompletedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required CheckoutEntity Object { get; set; }
}

public class WebhookRefundCreatedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required RefundEntity Object { get; set; }
}

public class WebhookDisputeCreatedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required DisputeEntity Object { get; set; }
}

public class WebhookCustomerCreditsExhaustedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required CustomerCreditsExhaustionEntity Object { get; set; }
}

public class WebhookCreditsGrantedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required CustomerCreditsGrantEntity Object { get; set; }
}

public class WebhookCreditsConsumedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required CustomerCreditsConsumptionEntity Object { get; set; }
}

public class WebhookCreditsAutoRechargedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required CustomerCreditsAutoRechargeEntity Object { get; set; }
}

public class WebhookSubscriptionActiveEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionTrialingEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionCanceledEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionScheduledCancelEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionPaidEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionExpiredEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionUnpaidEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionUpdateEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionPastDueEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

public class WebhookSubscriptionPausedEventEntity : WebhookEventEntity
{
    [JsonPropertyName("object")]
    public required WebhookSubscriptionEntity Object { get; set; }
}

/// <summary>
/// Subscription object carried by subscription webhook events.
/// </summary>
public class WebhookSubscriptionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("product")]
    public required ProductEntity Product { get; set; }

    [JsonPropertyName("customer")]
    public required CustomerEntity Customer { get; set; }

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
    public WebhookSubscriptionDiscountEntity? Discount { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, System.Text.Json.JsonElement>? Metadata { get; set; }
}

/// <summary>
/// The discount applied to a subscription in a webhook payload.
/// </summary>
public class WebhookSubscriptionDiscountEntity
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

/// <summary>
/// Reads a webhook payload into the event type named by its <c>eventType</c>
/// field. Unlike the built-in polymorphism support this does not require the
/// discriminator to be the first JSON property.
/// </summary>
public sealed class WebhookEventJsonConverter : JsonConverter<WebhookEventEntity>
{
    public override WebhookEventEntity? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (!root.TryGetProperty("eventType", out var eventTypeElement)
            && !root.TryGetProperty("event_type", out eventTypeElement))
        {
            throw new JsonException("Webhook payload does not contain an eventType field.");
        }

        var eventType = eventTypeElement.GetString();

        var targetType = eventType switch
        {
            "checkout.completed" => typeof(WebhookCheckoutCompletedEventEntity),
            "refund.created" => typeof(WebhookRefundCreatedEventEntity),
            "dispute.created" => typeof(WebhookDisputeCreatedEventEntity),
            "customer_credits.exhausted" => typeof(WebhookCustomerCreditsExhaustedEventEntity),
            "credits.granted" => typeof(WebhookCreditsGrantedEventEntity),
            "credits.consumed" => typeof(WebhookCreditsConsumedEventEntity),
            "credits.auto_recharged" => typeof(WebhookCreditsAutoRechargedEventEntity),
            "subscription.active" => typeof(WebhookSubscriptionActiveEventEntity),
            "subscription.trialing" => typeof(WebhookSubscriptionTrialingEventEntity),
            "subscription.canceled" => typeof(WebhookSubscriptionCanceledEventEntity),
            "subscription.scheduled_cancel" => typeof(WebhookSubscriptionScheduledCancelEventEntity),
            "subscription.paid" => typeof(WebhookSubscriptionPaidEventEntity),
            "subscription.expired" => typeof(WebhookSubscriptionExpiredEventEntity),
            "subscription.unpaid" => typeof(WebhookSubscriptionUnpaidEventEntity),
            "subscription.update" => typeof(WebhookSubscriptionUpdateEventEntity),
            "subscription.past_due" => typeof(WebhookSubscriptionPastDueEventEntity),
            "subscription.paused" => typeof(WebhookSubscriptionPausedEventEntity),
            _ => throw new JsonException($"Unsupported webhook event type '{eventType}'."),
        };

        return (WebhookEventEntity?)root.Deserialize(targetType, options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        WebhookEventEntity value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

#endregion
