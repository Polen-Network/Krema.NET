using System.Text.Json;
using System.Text.Json.Serialization;

namespace Krema.NET.Meters;

[JsonConverter(typeof(JsonStringEnumConverter<MeterAggregation>))]
public enum MeterAggregation
{
    [JsonStringEnumMemberName("count")] Count,
    [JsonStringEnumMemberName("sum")] Sum,
    [JsonStringEnumMemberName("average")] Average,
    [JsonStringEnumMemberName("min")] Min,
    [JsonStringEnumMemberName("max")] Max,
    [JsonStringEnumMemberName("unique")] Unique,
}

[JsonConverter(typeof(JsonStringEnumConverter<MeterStatus>))]
public enum MeterStatus
{
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("archived")] Archived,
}

[JsonConverter(typeof(JsonStringEnumConverter<MeterFilterConjunction>))]
public enum MeterFilterConjunction
{
    [JsonStringEnumMemberName("and")] And,
    [JsonStringEnumMemberName("or")] Or,
}

[JsonConverter(typeof(JsonStringEnumConverter<FilterOperator>))]
public enum FilterOperator
{
    [JsonStringEnumMemberName("equals")] Equals,
    [JsonStringEnumMemberName("not_equals")] NotEquals,
    [JsonStringEnumMemberName("gt")] Gt,
    [JsonStringEnumMemberName("gte")] Gte,
    [JsonStringEnumMemberName("lt")] Lt,
    [JsonStringEnumMemberName("lte")] Lte,
    [JsonStringEnumMemberName("contains")] Contains,
    [JsonStringEnumMemberName("not_contains")] NotContains,
}

/// <summary>
/// A usage meter.
/// </summary>
public class MeterApiResponseDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("store_id")]
    public required string StoreId { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("event_name")]
    public required string EventName { get; set; }

    [JsonPropertyName("aggregation")]
    public required MeterAggregation Aggregation { get; set; }

    [JsonPropertyName("aggregation_property")]
    public string? AggregationProperty { get; set; }

    [JsonPropertyName("filter")]
    public required MeterFilterApiResponseDto Filter { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("status")]
    public required MeterStatus Status { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required string UpdatedAt { get; set; }
}

/// <summary>
/// Paginated meter list.
/// </summary>
public class MeterListApiResponseDto
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("data")]
    public required List<MeterApiResponseDto> Data { get; set; }

    [JsonPropertyName("has_more")]
    public required bool HasMore { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/meters</c>.
/// </summary>
public class CreateMeterApiRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("event_name")]
    public required string EventName { get; set; }

    [JsonPropertyName("aggregation")]
    public required MeterAggregation Aggregation { get; set; }

    [JsonPropertyName("aggregation_property")]
    public string? AggregationProperty { get; set; }

    [JsonPropertyName("filter")]
    public MeterFilterApiRequestDto? Filter { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }
}

/// <summary>
/// Request body for <c>PATCH /v1/meters/{id}</c>.
/// </summary>
public class UpdateMeterApiRequestDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("aggregation")]
    public MeterAggregation? Aggregation { get; set; }

    [JsonPropertyName("aggregation_property")]
    public string? AggregationProperty { get; set; }

    [JsonPropertyName("filter")]
    public MeterFilterApiRequestDto? Filter { get; set; }
}

/// <summary>
/// Filter applied to meter events.
/// </summary>
public class MeterFilterApiRequestDto
{
    [JsonPropertyName("conjunction")]
    public required MeterFilterConjunction Conjunction { get; set; }

    [JsonPropertyName("clauses")]
    public required List<FilterClauseApiRequestDto> Clauses { get; set; }
}

/// <summary>
/// Filter returned by the API for a meter.
/// </summary>
public class MeterFilterApiResponseDto
{
    [JsonPropertyName("conjunction")]
    public required MeterFilterConjunction Conjunction { get; set; }

    [JsonPropertyName("clauses")]
    public required List<FilterClauseApiResponseDto> Clauses { get; set; }
}

/// <summary>
/// A single meter filter clause sent to the API.
/// </summary>
public class FilterClauseApiRequestDto
{
    [JsonPropertyName("property")]
    public required string Property { get; set; }

    [JsonPropertyName("operator")]
    public required FilterOperator Operator { get; set; }

    /// <summary>String, number or boolean value.</summary>
    [JsonPropertyName("value")]
    public required JsonElement Value { get; set; }
}

/// <summary>
/// A single meter filter clause returned by the API.
/// </summary>
public class FilterClauseApiResponseDto
{
    [JsonPropertyName("property")]
    public required string Property { get; set; }

    [JsonPropertyName("operator")]
    public required string Operator { get; set; }

    /// <summary>String, number or boolean value.</summary>
    [JsonPropertyName("value")]
    public required JsonElement Value { get; set; }
}

/// <summary>
/// Units consumed for a meter by a customer during a billing period.
/// </summary>
public class MeterConsumedUnitsApiResponseDto
{
    [JsonPropertyName("meter_id")]
    public required string MeterId { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("units")]
    public required string Units { get; set; }

    [JsonPropertyName("period_start")]
    public required string PeriodStart { get; set; }

    [JsonPropertyName("period_end")]
    public required string PeriodEnd { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("as_of")]
    public string? AsOf { get; set; }
}

/// <summary>
/// Result of previewing a meter definition against recent events.
/// </summary>
public class MeterPreviewApiResponseDto
{
    [JsonPropertyName("matched_events")]
    public required int MatchedEvents { get; set; }

    [JsonPropertyName("units")]
    public required string Units { get; set; }

    [JsonPropertyName("distinct_customers")]
    public required int DistinctCustomers { get; set; }

    [JsonPropertyName("daily")]
    public required List<MeterPreviewDailyApiResponseDto> Daily { get; set; }
}

/// <summary>
/// One day of a meter preview.
/// </summary>
public class MeterPreviewDailyApiResponseDto
{
    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("event_count")]
    public required int EventCount { get; set; }

    [JsonPropertyName("value")]
    public required string Value { get; set; }
}

/// <summary>
/// Request body for previewing a meter definition.
/// </summary>
public class PreviewMeterApiRequestDto
{
    [JsonPropertyName("event_name")]
    public required string EventName { get; set; }

    [JsonPropertyName("aggregation")]
    public required MeterAggregation Aggregation { get; set; }

    [JsonPropertyName("aggregation_property")]
    public string? AggregationProperty { get; set; }

    [JsonPropertyName("filter")]
    public MeterFilterApiRequestDto? Filter { get; set; }

    [JsonPropertyName("unit_label")]
    public string? UnitLabel { get; set; }
}
