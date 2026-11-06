using System.Text.Json;
using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;

namespace Krema.NET.Customers;

/// <summary>
/// A Creem customer.
/// </summary>
public class CustomerEntity
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

    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// Paginated customer list.
/// </summary>
public class CustomerListEntity
{
    [JsonPropertyName("items")]
    public required List<CustomerEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}

/// <summary>
/// Request body for <c>POST /v1/customers</c>.
/// </summary>
public class CreateCustomerRequestEntity
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }
}

/// <summary>
/// Request body for <c>PATCH /v1/customers</c>.
/// </summary>
public class UpdateCustomerRequestEntity
{
    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, JsonElement>? Metadata { get; set; }

    [JsonPropertyName("external_id")]
    public string? ExternalId { get; set; }
}

/// <summary>
/// Links related to a customer.
/// </summary>
public class CustomerLinksEntity
{
    [JsonPropertyName("customer_portal_link")]
    public required string CustomerPortalLink { get; set; }
}

/// <summary>
/// Request body for creating a customer portal link.
/// </summary>
public class CreateCustomerPortalLinkRequestEntity
{
    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }
}
