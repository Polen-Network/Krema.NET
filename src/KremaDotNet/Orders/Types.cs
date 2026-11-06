using System.Text.Json.Serialization;
using KremaDotNet.Checkout;
using KremaDotNet.Common;

namespace KremaDotNet.Orders;

/// <summary>
/// Paginated order list.
/// </summary>
public class OrderListEntity
{
    [JsonPropertyName("items")]
    public required List<OrderEntity> Items { get; set; }

    [JsonPropertyName("pagination")]
    public required PaginationEntity Pagination { get; set; }
}
