using System.Text.Json.Serialization;
using Krema.NET.Checkout;
using Krema.NET.Common;

namespace Krema.NET.Orders;

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
