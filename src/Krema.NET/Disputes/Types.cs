using System.Text.Json;
using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Transactions;

namespace Krema.NET.Disputes;

/// <summary>
/// A payment dispute.
/// </summary>
public class DisputeEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("mode")]
    public required EnvironmentMode Mode { get; set; }

    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    [JsonPropertyName("transaction")]
    public required TransactionEntity Transaction { get; set; }

    /// <summary>
    /// Either a checkout ID string or an expanded checkout object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("checkout")]
    public JsonElement? Checkout { get; set; }

    /// <summary>
    /// Either an order ID string or an expanded order object.
    /// Use <see cref="CreemJson.GetId"/> / <see cref="CreemJson.GetObject{T}"/>.
    /// </summary>
    [JsonPropertyName("order")]
    public JsonElement? Order { get; set; }

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

    [JsonPropertyName("created_at")]
    public required long CreatedAt { get; set; }
}
