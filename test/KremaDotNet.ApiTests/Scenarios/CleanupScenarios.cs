using System.Net.Http;
using KremaDotNet.Checkout;
using KremaDotNet.Discounts;
using KremaDotNet.Meters;
using KremaDotNet.Products;
using KremaDotNet.Splits;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Deletes or archives the resources created during the run so the test store stays tidy.
/// </summary>
internal static class CleanupScenarios
{
    /// <summary>Deletes every resource the run created, skipping ids that were never set.</summary>
    public static async Task RunAsync(TestRun run)
    {
        if (run.SplitId is not null)
        {
            await run.CallAsync<SplitEntity>(
                "Cleanup",
                "Delete split",
                HttpMethod.Delete,
                $"splits/{Uri.EscapeDataString(run.SplitId)}");
        }

        if (run.DiscountId is not null)
        {
            await run.CallAsync<DiscountEntity>(
                "Cleanup",
                "Delete discount",
                HttpMethod.Delete,
                $"discounts/{Uri.EscapeDataString(run.DiscountId)}/delete");
        }

        if (run.RecurringProductId is not null)
        {
            await run.CallAsync<ProductEntity>(
                "Cleanup",
                "Archive recurring product",
                HttpMethod.Delete,
                $"products/{Uri.EscapeDataString(run.RecurringProductId)}");
        }

        if (run.ProductId is not null)
        {
            await run.CallAsync<ProductEntity>(
                "Cleanup",
                "Archive one-time product",
                HttpMethod.Delete,
                $"products/{Uri.EscapeDataString(run.ProductId)}");
        }

        if (run.MeterId is not null)
        {
            await run.CallAsync<MeterApiResponseDto>(
                "Cleanup",
                "Archive meter",
                HttpMethod.Post,
                $"meters/{Uri.EscapeDataString(run.MeterId)}/archive");
        }
    }
}
