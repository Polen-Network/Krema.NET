using System.Text.Json;
using System.Text.Json.Serialization;

namespace Krema.NET.Usage;

[JsonConverter(typeof(JsonStringEnumConverter<IngestWarningCode>))]
public enum IngestWarningCode
{
    [JsonStringEnumMemberName("no_matching_meter")] NoMatchingMeter,
    [JsonStringEnumMemberName("meter_archived")] MeterArchived,
    [JsonStringEnumMemberName("timestamp_outside_late_window")] TimestampOutsideLateWindow,
}

[JsonConverter(typeof(JsonStringEnumConverter<PreviewWarningCode>))]
public enum PreviewWarningCode
{
    [JsonStringEnumMemberName("no_matching_meter")] NoMatchingMeter,
    [JsonStringEnumMemberName("meter_archived")] MeterArchived,
    [JsonStringEnumMemberName("timestamp_outside_late_window")] TimestampOutsideLateWindow,
}

/// <summary>
/// A single usage event sent to the ingestion endpoint.
/// </summary>
public class IngestUsageEventApiRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("external_customer_id")]
    public string? ExternalCustomerId { get; set; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, JsonElement>? Properties { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/usage-events</c>.
/// </summary>
public class IngestUsageEventsApiRequestDto
{
    [JsonPropertyName("events")]
    public required List<IngestUsageEventApiRequestDto> Events { get; set; }
}

/// <summary>
/// Response returned after ingesting usage events.
/// </summary>
public class IngestUsageEventsApiResponseDto
{
    [JsonPropertyName("accepted")]
    public required int Accepted { get; set; }

    [JsonPropertyName("event_ids")]
    public required List<string> EventIds { get; set; }

    [JsonPropertyName("warnings")]
    public List<IngestUsageEventWarningApiDto>? Warnings { get; set; }
}

/// <summary>
/// A non-fatal warning returned by event ingestion.
/// </summary>
public class IngestUsageEventWarningApiDto
{
    [JsonPropertyName("index")]
    public required int Index { get; set; }

    [JsonPropertyName("code")]
    public required IngestWarningCode Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }
}

/// <summary>
/// An error attached to a single event in a preview report.
/// </summary>
public class UsagePreviewError
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("param")]
    public string? Param { get; set; }
}

/// <summary>
/// Preview result for a single usage event.
/// </summary>
public class PreviewUsageEventReportApiDto
{
    [JsonPropertyName("valid")]
    public required bool Valid { get; set; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("error")]
    public UsagePreviewError? Error { get; set; }

    [JsonPropertyName("duplicate")]
    public bool? Duplicate { get; set; }

    [JsonPropertyName("matched_meters")]
    public required List<string> MatchedMeters { get; set; }

    [JsonPropertyName("warnings")]
    public required List<PreviewUsageEventWarningApiDto> Warnings { get; set; }
}

/// <summary>
/// Response returned by the usage event preview endpoint.
/// </summary>
public class PreviewUsageEventsApiResponseDto
{
    [JsonPropertyName("valid")]
    public required bool Valid { get; set; }

    [JsonPropertyName("events")]
    public required List<PreviewUsageEventReportApiDto> Events { get; set; }
}

/// <summary>
/// A non-fatal warning returned by the usage event preview endpoint.
/// </summary>
public class PreviewUsageEventWarningApiDto
{
    [JsonPropertyName("code")]
    public required PreviewWarningCode Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }
}

/// <summary>
/// A stored usage event.
/// </summary>
public class UsageEventApiResponseDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("event_id")]
    public required string EventId { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("properties")]
    public required Dictionary<string, JsonElement> Properties { get; set; }

    [JsonPropertyName("timestamp")]
    public required string Timestamp { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("matched_meters")]
    public required List<string> MatchedMeters { get; set; }
}

/// <summary>
/// Paginated usage event list.
/// </summary>
public class UsageEventListApiResponseDto
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("data")]
    public required List<UsageEventApiResponseDto> Data { get; set; }

    [JsonPropertyName("has_more")]
    public required bool HasMore { get; set; }
}
