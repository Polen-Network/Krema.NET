using System.Text.Json.Serialization;

namespace Krema.NET.Stats;

/// <summary>
/// Revenue metrics for one period.
/// </summary>
public class StatsMetricPeriodEntity
{
    [JsonPropertyName("timestamp")]
    public required long Timestamp { get; set; }

    [JsonPropertyName("grossRevenue")]
    public required decimal GrossRevenue { get; set; }

    [JsonPropertyName("netRevenue")]
    public required decimal NetRevenue { get; set; }
}

/// <summary>
/// Aggregate store metrics.
/// </summary>
public class StatsMetricTotalsEntity
{
    [JsonPropertyName("totalProducts")]
    public required int TotalProducts { get; set; }

    [JsonPropertyName("totalSubscriptions")]
    public required int TotalSubscriptions { get; set; }

    [JsonPropertyName("totalCustomers")]
    public required int TotalCustomers { get; set; }

    [JsonPropertyName("totalPayments")]
    public required int TotalPayments { get; set; }

    [JsonPropertyName("activeSubscriptions")]
    public required int ActiveSubscriptions { get; set; }

    [JsonPropertyName("totalRevenue")]
    public required decimal TotalRevenue { get; set; }

    [JsonPropertyName("totalNetRevenue")]
    public required decimal TotalNetRevenue { get; set; }

    [JsonPropertyName("netMonthlyRecurringRevenue")]
    public required decimal NetMonthlyRecurringRevenue { get; set; }

    [JsonPropertyName("monthlyRecurringRevenue")]
    public required decimal MonthlyRecurringRevenue { get; set; }
}

/// <summary>
/// Store statistics summary.
/// </summary>
public class StatsSummaryEntity
{
    [JsonPropertyName("totals")]
    public required StatsMetricTotalsEntity Totals { get; set; }

    [JsonPropertyName("periods")]
    public List<StatsMetricPeriodEntity>? Periods { get; set; }
}
