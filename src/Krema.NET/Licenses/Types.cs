using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;

namespace Krema.NET.Licenses;

/// <summary>
/// Paginated license key list.
/// </summary>
public class LicenseListEntity
{
    [JsonPropertyName("items")]
    public required List<LicenseEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// Paginated license instance list.
/// </summary>
public class LicenseInstanceListEntity
{
    [JsonPropertyName("items")]
    public required List<LicenseInstanceEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// Request body for activating a license key.
/// </summary>
public class ActivateLicenseRequestEntity
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("instance_name")]
    public required string InstanceName { get; set; }
}

/// <summary>
/// Request body for deactivating a license key.
/// </summary>
public class DeactivateLicenseRequestEntity
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("instance_id")]
    public required string InstanceId { get; set; }
}

/// <summary>
/// Request body for validating a license key.
/// </summary>
public class ValidateLicenseRequestEntity
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("instance_id")]
    public required string InstanceId { get; set; }
}
