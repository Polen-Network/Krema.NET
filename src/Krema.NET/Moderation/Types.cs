using System.Text.Json.Serialization;

namespace Krema.NET.Moderation;

[JsonConverter(typeof(JsonStringEnumConverter<PromptDecision>))]
public enum PromptDecision
{
    [JsonStringEnumMemberName("allow")] Allow,
    [JsonStringEnumMemberName("deny")] Deny,
    [JsonStringEnumMemberName("flag")] Flag,
}

/// <summary>
/// Request body for the prompt screening endpoint.
/// </summary>
public class ScreenPromptRequest
{
    [JsonPropertyName("prompt")]
    public required string Prompt { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }
}

/// <summary>
/// Usage metadata attached to a moderation response.
/// </summary>
public class UsageEntity
{
    [JsonPropertyName("units")]
    public required decimal Units { get; set; }
}

/// <summary>
/// Response returned by the prompt screening endpoint.
/// </summary>
public class ScreenPromptResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("prompt")]
    public required string Prompt { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("decision")]
    public required PromptDecision Decision { get; set; }

    [JsonPropertyName("usage")]
    public required UsageEntity Usage { get; set; }
}
