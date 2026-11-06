using System.Text.Json;
using System.Text.Json.Serialization;

namespace Krema.NET.Checkout;

/// <summary>
/// Shared JSON helpers for Creem payloads.
/// </summary>
public static class CreemJson
{
    /// <summary>
    /// Default serializer options. Optional members are omitted instead of being
    /// written as <c>null</c>, which is what the Creem API expects.
    /// </summary>
    public static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Reads a <c>string | object</c> union member (for example <c>product</c>,
    /// <c>customer</c> or <c>subscription</c>) as an ID.
    /// Returns <c>null</c> when the API expanded the value into an object.
    /// </summary>
    public static string? GetId(JsonElement? value)
    {
        if (value is not { } element)
        {
            return null;
        }

        return element.ValueKind == JsonValueKind.String
            ? element.GetString()
            : null;
    }

    /// <summary>
    /// Deserializes an expanded <c>string | object</c> union member.
    /// Returns <c>null</c> when the API returned a plain ID string.
    /// </summary>
    public static T? GetObject<T>(JsonElement? value)
    {
        if (value is not { } element || element.ValueKind != JsonValueKind.Object)
        {
            return default;
        }

        return element.Deserialize<T>(Options);
    }
}

#region Enums

[JsonConverter(typeof(JsonStringEnumConverter<EnvironmentMode>))]
public enum EnvironmentMode
{
    [JsonStringEnumMemberName("test")] Test,
    [JsonStringEnumMemberName("prod")] Prod,
    [JsonStringEnumMemberName("sandbox")] Sandbox,
}

[JsonConverter(typeof(JsonStringEnumConverter<CheckoutStatus>))]
public enum CheckoutStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("processing")] Processing,
    [JsonStringEnumMemberName("completed")] Completed,
    [JsonStringEnumMemberName("expired")] Expired,
}

[JsonConverter(typeof(JsonStringEnumConverter<OrderStatus>))]
public enum OrderStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("paid")] Paid,
}

[JsonConverter(typeof(JsonStringEnumConverter<OrderType>))]
public enum OrderType
{
    [JsonStringEnumMemberName("recurring")] Recurring,
    [JsonStringEnumMemberName("onetime")] Onetime,
}

[JsonConverter(typeof(JsonStringEnumConverter<CustomFieldType>))]
public enum CustomFieldType
{
    [JsonStringEnumMemberName("text")] Text,
    [JsonStringEnumMemberName("checkbox")] Checkbox,
}

[JsonConverter(typeof(JsonStringEnumConverter<CheckoutDiscountType>))]
public enum CheckoutDiscountType
{
    [JsonStringEnumMemberName("percentage")] Percentage,
    [JsonStringEnumMemberName("fixed")] Fixed,
}

[JsonConverter(typeof(JsonStringEnumConverter<CheckoutDiscountDuration>))]
public enum CheckoutDiscountDuration
{
    [JsonStringEnumMemberName("forever")] Forever,
    [JsonStringEnumMemberName("once")] Once,
    [JsonStringEnumMemberName("repeating")] Repeating,
}

[JsonConverter(typeof(JsonStringEnumConverter<LicenseStatus>))]
public enum LicenseStatus
{
    [JsonStringEnumMemberName("inactive")] Inactive,
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("expired")] Expired,
    [JsonStringEnumMemberName("disabled")] Disabled,
}

[JsonConverter(typeof(JsonStringEnumConverter<LicenseInstanceStatus>))]
public enum LicenseInstanceStatus
{
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("deactivated")] Deactivated,
}

#endregion

#region Create checkout request

/// <summary>
/// Request body for <c>POST /v1/checkouts</c>.
/// </summary>
public class CreateCheckoutRequest
{
    /// <summary>Identify and track each checkout request.</summary>
    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }

    /// <summary>The ID of the product associated with the checkout session.</summary>
    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    /// <summary>The number of units for the order (API default: 1).</summary>
    [JsonPropertyName("units")]
    public int? Units { get; set; }

    /// <summary>
    /// Per-unit price override in cents (only for one-time payment products).
    /// Must be between 100 and 99999999.
    /// </summary>
    [JsonPropertyName("custom_price")]
    public int? CustomPrice { get; set; }

    /// <summary>Prefill the checkout session with a discount code.</summary>
    [JsonPropertyName("discount_code")]
    public string? DiscountCode { get; set; }

    /// <summary>Attribute this checkout to an affiliate using their referral code.</summary>
    [JsonPropertyName("affiliate_code")]
    public string? AffiliateCode { get; set; }

    /// <summary>Customer data used to prefill the checkout page.</summary>
    [JsonPropertyName("customer")]
    public CustomerRequestEntity? Customer { get; set; }

    /// <summary>Additional information to collect. Up to 3 fields are supported.</summary>
    [JsonPropertyName("custom_fields")]
    public List<CustomFieldRequestEntity>? CustomFields { get; set; }

    /// <summary>DEPRECATED: use <see cref="CustomFields"/> instead.</summary>
    [JsonPropertyName("custom_field")]
    [Obsolete("Use CustomFields (custom_fields) instead.")]
    public List<CustomFieldRequestEntity>? CustomField { get; set; }

    /// <summary>URL the customer is redirected to after the checkout completes.</summary>
    [JsonPropertyName("success_url")]
    public string? SuccessUrl { get; set; }

    /// <summary>Metadata for the checkout in the form of key-value pairs.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }
}

/// <summary>
/// Customer data used to prefill a checkout. Only one of <see cref="Id"/> or
/// <see cref="Email"/> may be set.
/// </summary>
public class CustomerRequestEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

/// <summary>
/// Custom field definition sent when creating a checkout.
/// </summary>
public class CustomFieldRequestEntity
{
    [JsonPropertyName("type")]
    public required CustomFieldType Type { get; set; }

    /// <summary>Unique key, alphanumeric, up to 200 characters.</summary>
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    /// <summary>Label displayed to the customer, up to 50 characters.</summary>
    [JsonPropertyName("label")]
    public required string Label { get; set; }

    /// <summary>Whether the customer is required to complete the field.</summary>
    [JsonPropertyName("optional")]
    public bool? Optional { get; set; }

    [JsonPropertyName("text")]
    public TextFieldConfig? Text { get; set; }

    [JsonPropertyName("checkbox")]
    public CheckboxFieldConfig? Checkbox { get; set; }
}

/// <summary>
/// Request-side configuration for a <c>text</c> custom field.
/// </summary>
public class TextFieldConfig
{
    [JsonPropertyName("max_length")]
    public int? MaxLength { get; set; }

    [JsonPropertyName("min_length")]
    public int? MinLength { get; set; }
}

/// <summary>
/// Request-side configuration for a <c>checkbox</c> custom field.
/// </summary>
public class CheckboxFieldConfig
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

#endregion

#region Checkout response

/// <summary>
/// Response returned by <c>POST /v1/checkouts</c> and <c>GET /v1/checkouts</c>.
/// </summary>
public class CheckoutEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("status")]
    public required CheckoutStatus Status { get; set; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }

    /// <summary>
    /// Either a product ID string or an expanded product object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("product")]
    public required JsonElement Product { get; set; }

    [JsonPropertyName("units")]
    public int Units { get; set; } = 1;

    /// <summary>Per-unit price override, present only when set on creation.</summary>
    [JsonPropertyName("custom_price")]
    public int? CustomPrice { get; set; }

    [JsonPropertyName("order")]
    public OrderEntity? Order { get; set; }

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

    [JsonPropertyName("custom_fields")]
    public List<CustomFieldEntity>? CustomFields { get; set; }

    [JsonPropertyName("checkout_url")]
    public string? CheckoutUrl { get; set; }

    [JsonPropertyName("success_url")]
    public string? SuccessUrl { get; set; }

    [JsonPropertyName("license_keys")]
    public List<LicenseEntity>? LicenseKeys { get; set; }

    /// <summary>DEPRECATED: use <see cref="LicenseKeys"/> instead.</summary>
    [JsonPropertyName("feature")]
    [Obsolete("Use LicenseKeys (license_keys) instead.")]
    public JsonElement? Feature { get; set; }

    /// <summary>Metadata for the checkout in the form of key-value pairs.</summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }

    [JsonPropertyName("discount")]
    public CheckoutDiscountEntity? Discount { get; set; }
}

/// <summary>
/// The discount applied to a checkout.
/// </summary>
/// <remarks>
/// Creem returns this object with camelCase keys (<c>discountCode</c>,
/// <c>durationInMonths</c>), unlike the rest of the API.
/// </remarks>
public class CheckoutDiscountEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("discountCode")]
    public string? DiscountCode { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public CheckoutDiscountType? Type { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("duration")]
    public CheckoutDiscountDuration? Duration { get; set; }

    [JsonPropertyName("durationInMonths")]
    public decimal? DurationInMonths { get; set; }
}

/// <summary>
/// Custom field value collected during checkout.
/// </summary>
public class CustomFieldEntity
{
    [JsonPropertyName("type")]
    public required CustomFieldType Type { get; set; }

    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("label")]
    public required string Label { get; set; }

    [JsonPropertyName("optional")]
    public bool? Optional { get; set; }

    [JsonPropertyName("text")]
    public CustomFieldText? Text { get; set; }

    [JsonPropertyName("checkbox")]
    public CustomFieldCheckbox? Checkbox { get; set; }
}

/// <summary>
/// Response-side value of a <c>text</c> custom field.
/// </summary>
/// <remarks>
/// The API uses <c>minimum_length</c> on responses and <c>min_length</c> on
/// requests, hence the separate models.
/// </remarks>
public class CustomFieldText
{
    [JsonPropertyName("max_length")]
    public int? MaxLength { get; set; }

    [JsonPropertyName("minimum_length")]
    public int? MinimumLength { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

/// <summary>
/// Response-side value of a <c>checkbox</c> custom field.
/// </summary>
public class CustomFieldCheckbox
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("value")]
    public bool? Value { get; set; }
}

#endregion

#region Order

/// <summary>
/// The order associated with a checkout.
/// </summary>
public class OrderEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    /// <summary>ID of the customer who placed the order.</summary>
    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    /// <summary>ID of the product associated with the order.</summary>
    [JsonPropertyName("product")]
    public required string Product { get; set; }

    [JsonPropertyName("transaction")]
    public string? Transaction { get; set; }

    /// <summary>ID of the discount applied to the order.</summary>
    [JsonPropertyName("discount")]
    public string? Discount { get; set; }

    /// <summary>Total amount in cents.</summary>
    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("sub_total")]
    public decimal? SubTotal { get; set; }

    [JsonPropertyName("tax_amount")]
    public decimal? TaxAmount { get; set; }

    [JsonPropertyName("discount_amount")]
    public decimal? DiscountAmount { get; set; }

    [JsonPropertyName("amount_due")]
    public decimal? AmountDue { get; set; }

    [JsonPropertyName("amount_paid")]
    public decimal? AmountPaid { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("fx_amount")]
    public decimal? FxAmount { get; set; }

    [JsonPropertyName("fx_currency")]
    public string? FxCurrency { get; set; }

    [JsonPropertyName("fx_rate")]
    public decimal? FxRate { get; set; }

    [JsonPropertyName("status")]
    public required OrderStatus Status { get; set; }

    [JsonPropertyName("type")]
    public required OrderType Type { get; set; }

    [JsonPropertyName("affiliate")]
    public string? Affiliate { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTimeOffset UpdatedAt { get; set; }
}

#endregion

#region Licenses

/// <summary>
/// License key issued for an order.
/// </summary>
public class LicenseEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    [JsonPropertyName("status")]
    public required LicenseStatus Status { get; set; }

    [JsonPropertyName("key")]
    public required string Key { get; set; }

    /// <summary>Number of instances this license key was activated on.</summary>
    [JsonPropertyName("activation")]
    public int Activation { get; set; }

    /// <summary>Null when activations are unlimited.</summary>
    [JsonPropertyName("activation_limit")]
    public int? ActivationLimit { get; set; }

    /// <summary>Null when the license does not expire.</summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset? ExpiresAt { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("instance")]
    public LicenseInstanceEntity? Instance { get; set; }
}

/// <summary>
/// A single activation of a license key.
/// </summary>
public class LicenseInstanceEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("status")]
    public required LicenseInstanceStatus Status { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }
}

#endregion
