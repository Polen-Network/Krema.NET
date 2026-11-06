using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;

namespace Krema.NET.Splits;

[JsonConverter(typeof(JsonStringEnumConverter<SplitEntityType>))]
public enum SplitEntityType
{
    [JsonStringEnumMemberName("store")] Store,
    [JsonStringEnumMemberName("product")] Product,
}

[JsonConverter(typeof(JsonStringEnumConverter<SplitRecipientShareType>))]
public enum SplitRecipientShareType
{
    [JsonStringEnumMemberName("fixed")] Fixed,
    [JsonStringEnumMemberName("percentage")] Percentage,
}

[JsonConverter(typeof(JsonStringEnumConverter<SplitRecipientKind>))]
public enum SplitRecipientKind
{
    [JsonStringEnumMemberName("store")] Store,
    [JsonStringEnumMemberName("user")] User,
    [JsonStringEnumMemberName("email")] Email,
}

[JsonConverter(typeof(JsonStringEnumConverter<SplitInviteStatus>))]
public enum SplitInviteStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("declined")] Declined,
    [JsonStringEnumMemberName("revoked")] Revoked,
}

[JsonConverter(typeof(JsonStringEnumConverter<SplitRecipientRequestKind>))]
public enum SplitRecipientRequestKind
{
    [JsonStringEnumMemberName("store")] Store,
    [JsonStringEnumMemberName("email")] Email,
}

/// <summary>
/// A revenue split between the store and its recipients.
/// </summary>
public class SplitEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("store_id")]
    public required string StoreId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public required SplitEntityType Type { get; set; }

    [JsonPropertyName("type_reference")]
    public required string TypeReference { get; set; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; set; }

    [JsonPropertyName("recipients")]
    public required List<SplitRecipientEntity> Recipients { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

/// <summary>
/// Paginated split list.
/// </summary>
public class SplitListEntity
{
    [JsonPropertyName("items")]
    public required List<SplitEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// A recipient of a revenue split.
/// </summary>
public class SplitRecipientEntity
{
    [JsonPropertyName("type")]
    public required SplitRecipientShareType Type { get; set; }

    [JsonPropertyName("recipient_type")]
    public required SplitRecipientKind RecipientType { get; set; }

    [JsonPropertyName("recipient_reference")]
    public required string RecipientReference { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; set; }

    [JsonPropertyName("invite_status")]
    public SplitInviteStatus? InviteStatus { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/splits</c>.
/// </summary>
public class CreateSplitRequestEntity
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public required SplitEntityType Type { get; set; }

    [JsonPropertyName("type_reference")]
    public required string TypeReference { get; set; }

    [JsonPropertyName("recipients")]
    public required List<CreateSplitRecipientRequestEntity> Recipients { get; set; }
}

/// <summary>
/// A split recipient sent in a create split request.
/// </summary>
public class CreateSplitRecipientRequestEntity
{
    [JsonPropertyName("recipient_type")]
    public required SplitRecipientRequestKind RecipientType { get; set; }

    [JsonPropertyName("recipient_reference")]
    public required string RecipientReference { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }
}
