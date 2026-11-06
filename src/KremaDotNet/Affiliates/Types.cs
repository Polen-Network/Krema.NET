using System.Text.Json.Serialization;
using KremaDotNet.Checkout;
using KremaDotNet.Common;

namespace KremaDotNet.Affiliates;

[JsonConverter(typeof(JsonStringEnumConverter<CommissionStatus>))]
public enum CommissionStatus
{
    [JsonStringEnumMemberName("pending")] Pending,
    [JsonStringEnumMemberName("approved")] Approved,
    [JsonStringEnumMemberName("paid")] Paid,
}

/// <summary>
/// An affiliate of the store.
/// </summary>
public class AffiliateEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("referral_code")]
    public required string ReferralCode { get; set; }

    [JsonPropertyName("referral_link")]
    public required string ReferralLink { get; set; }

    [JsonPropertyName("clicks")]
    public required int Clicks { get; set; }

    [JsonPropertyName("conversions")]
    public required int Conversions { get; set; }

    [JsonPropertyName("earnings")]
    public required decimal Earnings { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
}

/// <summary>
/// Paginated affiliate list.
/// </summary>
public class AffiliateListEntity
{
    [JsonPropertyName("items")]
    public required List<AffiliateEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// An invitation sent to a potential affiliate.
/// </summary>
public class AffiliateInviteEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

/// <summary>
/// Paginated affiliate invite list.
/// </summary>
public class AffiliateInviteListEntity
{
    [JsonPropertyName("items")]
    public required List<AffiliateInviteEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// Request body for inviting an affiliate.
/// </summary>
public class CreateAffiliateInviteRequestEntity
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("program_id")]
    public string? ProgramId { get; set; }
}

/// <summary>
/// A commission earned by an affiliate.
/// </summary>
public class CommissionEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("affiliate")]
    public required string Affiliate { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("status")]
    public required CommissionStatus Status { get; set; }

    [JsonPropertyName("sale")]
    public string? Sale { get; set; }

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}

/// <summary>
/// Paginated commission list.
/// </summary>
public class CommissionListEntity
{
    [JsonPropertyName("items")]
    public required List<CommissionEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}
