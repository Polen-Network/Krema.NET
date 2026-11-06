using System.Text.Json.Serialization;
using KremaDotNet.Checkout;
using KremaDotNet.Common;

namespace KremaDotNet.Products;

#region Enums

[JsonConverter(typeof(JsonStringEnumConverter<ProductCurrency>))]
public enum ProductCurrency
{
    [JsonStringEnumMemberName("EUR")] Eur,
    [JsonStringEnumMemberName("USD")] Usd,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductBillingType>))]
public enum ProductBillingType
{
    [JsonStringEnumMemberName("recurring")] Recurring,
    [JsonStringEnumMemberName("onetime")] Onetime,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductBillingPeriod>))]
public enum ProductBillingPeriod
{
    [JsonStringEnumMemberName("every-month")] EveryMonth,
    [JsonStringEnumMemberName("every-three-months")] EveryThreeMonths,
    [JsonStringEnumMemberName("every-six-months")] EverySixMonths,
    [JsonStringEnumMemberName("every-year")] EveryYear,
    [JsonStringEnumMemberName("every-day")] EveryDay,
    [JsonStringEnumMemberName("once")] Once,
    [JsonStringEnumMemberName("custom")] Custom,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductRequestBillingType>))]
public enum ProductRequestBillingType
{
    [JsonStringEnumMemberName("recurring")] Recurring,
    [JsonStringEnumMemberName("onetime")] Onetime,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductRequestBillingPeriod>))]
public enum ProductRequestBillingPeriod
{
    [JsonStringEnumMemberName("once")] Once,
    [JsonStringEnumMemberName("every-day")] EveryDay,
    [JsonStringEnumMemberName("every-month")] EveryMonth,
    [JsonStringEnumMemberName("every-three-months")] EveryThreeMonths,
    [JsonStringEnumMemberName("every-six-months")] EverySixMonths,
    [JsonStringEnumMemberName("every-year")] EveryYear,
    [JsonStringEnumMemberName("custom")] Custom,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductRequestRecurringInterval>))]
public enum ProductRequestRecurringInterval
{
    [JsonStringEnumMemberName("day")] Day,
    [JsonStringEnumMemberName("week")] Week,
    [JsonStringEnumMemberName("month")] Month,
    [JsonStringEnumMemberName("year")] Year,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductStatus>))]
public enum ProductStatus
{
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("archived")] Archived,
}

[JsonConverter(typeof(JsonStringEnumConverter<TaxMode>))]
public enum TaxMode
{
    [JsonStringEnumMemberName("inclusive")] Inclusive,
    [JsonStringEnumMemberName("exclusive")] Exclusive,
}

[JsonConverter(typeof(JsonStringEnumConverter<TaxCategory>))]
public enum TaxCategory
{
    [JsonStringEnumMemberName("saas")] Saas,
    [JsonStringEnumMemberName("digital-goods-service")] DigitalGoodsService,
    [JsonStringEnumMemberName("ebooks")] Ebooks,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductFeatureType>))]
public enum ProductFeatureType
{
    [JsonStringEnumMemberName("custom")] Custom,
    [JsonStringEnumMemberName("file")] File,
    [JsonStringEnumMemberName("licenseKey")] LicenseKey,
    [JsonStringEnumMemberName("customerCredits")] CustomerCredits,
}

[JsonConverter(typeof(JsonStringEnumConverter<ProductFeatureRequestType>))]
public enum ProductFeatureRequestType
{
    [JsonStringEnumMemberName("customerCredits")] CustomerCredits,
}

[JsonConverter(typeof(JsonStringEnumConverter<UsagePriceSettlementMode>))]
public enum UsagePriceSettlementMode
{
    [JsonStringEnumMemberName("prepaid")] Prepaid,
    [JsonStringEnumMemberName("postpaid")] Postpaid,
}

[JsonConverter(typeof(JsonStringEnumConverter<UsagePriceTargetAccount>))]
public enum UsagePriceTargetAccount
{
    [JsonStringEnumMemberName("shared")] Shared,
    [JsonStringEnumMemberName("per_unit")] PerUnit,
}

[JsonConverter(typeof(JsonStringEnumConverter<UsagePriceTrialUsageMode>))]
public enum UsagePriceTrialUsageMode
{
    [JsonStringEnumMemberName("free")] Free,
    [JsonStringEnumMemberName("accrue")] Accrue,
}

#endregion

#region Product responses

/// <summary>
/// A Creem product.
/// </summary>
public class ProductEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("image_urls")]
    public List<string>? ImageUrls { get; set; }

    [JsonPropertyName("features")]
    public List<FeatureEntity>? Features { get; set; }

    [JsonPropertyName("usage_prices")]
    public List<UsagePriceEntity>? UsagePrices { get; set; }

    /// <summary>Price in cents.</summary>
    [JsonPropertyName("price")]
    public required decimal Price { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("billing_type")]
    public required ProductBillingType BillingType { get; set; }

    [JsonPropertyName("billing_period")]
    public required ProductBillingPeriod BillingPeriod { get; set; }

    [JsonPropertyName("recurring_interval")]
    public string? RecurringInterval { get; set; }

    [JsonPropertyName("recurring_interval_count")]
    public int? RecurringIntervalCount { get; set; }

    [JsonPropertyName("trial_period_days")]
    public int? TrialPeriodDays { get; set; }

    [JsonPropertyName("trial_price")]
    public decimal? TrialPrice { get; set; }

    [JsonPropertyName("status")]
    public required ProductStatus Status { get; set; }

    [JsonPropertyName("tax_mode")]
    public required TaxMode TaxMode { get; set; }

    [JsonPropertyName("business_net_pricing")]
    public bool? BusinessNetPricing { get; set; }

    [JsonPropertyName("tax_category")]
    public required TaxCategory TaxCategory { get; set; }

    [JsonPropertyName("product_url")]
    public string? ProductUrl { get; set; }

    [JsonPropertyName("default_success_url")]
    public string? DefaultSuccessUrl { get; set; }

    [JsonPropertyName("custom_fields")]
    public List<CustomFieldEntity>? CustomFields { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Paginated product list.
/// </summary>
public class ProductListEntity
{
    [JsonPropertyName("items")]
    public required List<ProductEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// A feature attached to a product.
/// </summary>
public class FeatureEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("type")]
    public required ProductFeatureType Type { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("customer_credits")]
    public CustomerCreditsFeatureEntity? CustomerCredits { get; set; }
}

/// <summary>
/// Credits granted by a product feature.
/// </summary>
public class CustomerCreditsFeatureEntity
{
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("unit_label")]
    public string? UnitLabel { get; set; }

    [JsonPropertyName("bucket_name")]
    public string? BucketName { get; set; }
}

/// <summary>
/// A file attached to a product feature.
/// </summary>
public class FeatureFileEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("file_name")]
    public required string FileName { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("size")]
    public required int Size { get; set; }
}

/// <summary>
/// Usage-based price attached to a product.
/// </summary>
public class UsagePriceEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("meter_id")]
    public required string MeterId { get; set; }

    [JsonPropertyName("unit_price")]
    public required decimal UnitPrice { get; set; }

    [JsonPropertyName("free_allowance")]
    public required decimal FreeAllowance { get; set; }

    [JsonPropertyName("cap")]
    public decimal? Cap { get; set; }

    [JsonPropertyName("settlement_mode")]
    public required UsagePriceSettlementMode SettlementMode { get; set; }

    [JsonPropertyName("target_account")]
    public required UsagePriceTargetAccount TargetAccount { get; set; }

    [JsonPropertyName("target_account_name")]
    public string? TargetAccountName { get; set; }

    [JsonPropertyName("trial_usage_mode")]
    public required UsagePriceTrialUsageMode TrialUsageMode { get; set; }
}

#endregion

#region Product requests

/// <summary>
/// Request body for <c>POST /v1/products</c>.
/// </summary>
public class CreateProductRequestEntity
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("image_urls")]
    public List<string>? ImageUrls { get; set; }

    /// <summary>Price in cents.</summary>
    [JsonPropertyName("price")]
    public required decimal Price { get; set; }

    [JsonPropertyName("currency")]
    public required ProductCurrency Currency { get; set; }

    [JsonPropertyName("billing_type")]
    public required ProductRequestBillingType BillingType { get; set; }

    [JsonPropertyName("billing_period")]
    public ProductRequestBillingPeriod? BillingPeriod { get; set; }

    [JsonPropertyName("recurring_interval")]
    public ProductRequestRecurringInterval? RecurringInterval { get; set; }

    [JsonPropertyName("recurring_interval_count")]
    public int? RecurringIntervalCount { get; set; }

    [JsonPropertyName("tax_mode")]
    public TaxMode? TaxMode { get; set; }

    [JsonPropertyName("business_net_pricing")]
    public bool? BusinessNetPricing { get; set; }

    [JsonPropertyName("tax_category")]
    public TaxCategory? TaxCategory { get; set; }

    [JsonPropertyName("pay_what_you_want")]
    public bool? PayWhatYouWant { get; set; }

    [JsonPropertyName("suggested_price")]
    public decimal? SuggestedPrice { get; set; }

    [JsonPropertyName("trial_period_days")]
    public int? TrialPeriodDays { get; set; }

    [JsonPropertyName("trial_price")]
    public decimal? TrialPrice { get; set; }

    [JsonPropertyName("default_success_url")]
    public string? DefaultSuccessUrl { get; set; }

    [JsonPropertyName("custom_fields")]
    public List<CustomFieldRequestEntity>? CustomFields { get; set; }

    /// <summary>DEPRECATED: use <see cref="CustomFields"/> instead.</summary>
    [JsonPropertyName("custom_field")]
    [Obsolete("Use CustomFields (custom_fields) instead.")]
    public List<CustomFieldRequestEntity>? CustomField { get; set; }

    [JsonPropertyName("abandoned_cart_recovery_enabled")]
    public bool? AbandonedCartRecoveryEnabled { get; set; }

    [JsonPropertyName("usage_prices")]
    public List<UsagePriceRequestEntity>? UsagePrices { get; set; }

    [JsonPropertyName("features")]
    public List<ProductFeatureRequestEntity>? Features { get; set; }
}

/// <summary>
/// Request body for <c>PATCH /v1/products/{id}</c>.
/// </summary>
public class UpdateProductRequestEntity
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("image_urls")]
    public List<string>? ImageUrls { get; set; }

    [JsonPropertyName("default_success_url")]
    public string? DefaultSuccessUrl { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("currency")]
    public ProductCurrency? Currency { get; set; }

    [JsonPropertyName("billing_type")]
    public ProductRequestBillingType? BillingType { get; set; }

    [JsonPropertyName("billing_period")]
    public ProductRequestBillingPeriod? BillingPeriod { get; set; }

    [JsonPropertyName("recurring_interval")]
    public ProductRequestRecurringInterval? RecurringInterval { get; set; }

    [JsonPropertyName("recurring_interval_count")]
    public int? RecurringIntervalCount { get; set; }

    [JsonPropertyName("tax_mode")]
    public TaxMode? TaxMode { get; set; }

    [JsonPropertyName("business_net_pricing")]
    public bool? BusinessNetPricing { get; set; }

    [JsonPropertyName("trial_period_days")]
    public int? TrialPeriodDays { get; set; }

    [JsonPropertyName("trial_price")]
    public decimal? TrialPrice { get; set; }

    [JsonPropertyName("pay_what_you_want")]
    public bool? PayWhatYouWant { get; set; }

    [JsonPropertyName("suggested_price")]
    public decimal? SuggestedPrice { get; set; }

    [JsonPropertyName("usage_prices")]
    public List<UsagePriceRequestEntity>? UsagePrices { get; set; }

    [JsonPropertyName("features")]
    public List<ProductFeatureRequestEntity>? Features { get; set; }
}

/// <summary>
/// A usage-based price sent in a create or update product request.
/// </summary>
public class UsagePriceRequestEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("meter_id")]
    public required string MeterId { get; set; }

    [JsonPropertyName("unit_price")]
    public required decimal UnitPrice { get; set; }

    [JsonPropertyName("free_allowance")]
    public decimal? FreeAllowance { get; set; }

    [JsonPropertyName("cap")]
    public decimal? Cap { get; set; }

    [JsonPropertyName("settlement_mode")]
    public required UsagePriceSettlementMode SettlementMode { get; set; }

    [JsonPropertyName("target_account")]
    public UsagePriceTargetAccount? TargetAccount { get; set; }

    [JsonPropertyName("target_account_name")]
    public string? TargetAccountName { get; set; }

    [JsonPropertyName("trial_usage_mode")]
    public UsagePriceTrialUsageMode? TrialUsageMode { get; set; }
}

/// <summary>
/// A product feature sent in a create or update product request.
/// </summary>
public class ProductFeatureRequestEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public required ProductFeatureRequestType Type { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("customer_credits")]
    public required CustomerCreditsFeatureEntity CustomerCredits { get; set; }
}

#endregion
