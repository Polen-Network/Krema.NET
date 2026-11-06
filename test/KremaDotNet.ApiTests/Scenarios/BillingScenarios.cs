using System.Net.Http;
using KremaDotNet.Subscriptions;
using KremaDotNet.Transactions;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Runs the read-mostly billing scenarios: transactions and subscriptions.
/// Nothing here refunds a payment or cancels/upgrades a subscription.
/// </summary>
internal static class BillingScenarios
{
    /// <summary>Runs all billing scenarios in order.</summary>
    public static async Task RunAsync(TestRun run)
    {
        await TransactionsAsync(run);
        await SubscriptionsAsync(run);
    }

    /// <summary>Searches transactions and reads the first one.</summary>
    private static async Task TransactionsAsync(TestRun run)
    {
        var transactions = await run.CallAsync<TransactionListEntity>(
            "Transactions",
            "Search transactions",
            HttpMethod.Get,
            "transactions/search?page_number=1&page_size=10");

        var first = transactions?.Items.FirstOrDefault();

        if (first is null)
        {
            run.Skip("Transactions", "Get transaction", "No transactions exist in the test account.");
            return;
        }

        await run.CallAsync<TransactionEntity>(
            "Transactions",
            "Get transaction",
            HttpMethod.Get,
            $"transactions?transaction_id={Uri.EscapeDataString(first.Id)}");
    }

    /// <summary>Searches subscriptions and reads, updates, pauses and resumes the first one.</summary>
    private static async Task SubscriptionsAsync(TestRun run)
    {
        var subscriptions = await run.CallAsync<SubscriptionListEntity>(
            "Subscriptions",
            "Search subscriptions",
            HttpMethod.Get,
            "subscriptions/search?page_number=1&page_size=10");

        var subscription = subscriptions?.Items.FirstOrDefault();

        if (subscription is null)
        {
            run.Skip("Subscriptions", "Get subscription", "No subscriptions exist in the test account.");
            run.Skip("Subscriptions", "Update subscription", "No subscriptions exist in the test account.");
            run.Skip("Subscriptions", "Pause subscription", "No subscriptions exist in the test account.");
            run.Skip("Subscriptions", "Resume subscription", "No subscriptions exist in the test account.");
            return;
        }

        run.SubscriptionId = subscription.Id;
        var subscriptionId = Uri.EscapeDataString(subscription.Id);

        var fullSubscription = await run.CallAsync<SubscriptionEntity>(
            "Subscriptions",
            "Get subscription",
            HttpMethod.Get,
            $"subscriptions?subscription_id={subscriptionId}");

        var items = fullSubscription?.Items ?? subscription.Items;

        if (items is null || items.Count == 0)
        {
            run.Skip("Subscriptions", "Update subscription", "Subscription has no items.");
        }
        else
        {
            await run.CallAsync<SubscriptionEntity>(
                "Subscriptions",
                "Update subscription",
                HttpMethod.Post,
                $"subscriptions/{subscriptionId}",
                new UpdateSubscriptionRequestEntity
                {
                    Items = items
                        .Select(item => new UpsertSubscriptionItemEntity { Id = item.Id })
                        .ToList(),
                });
        }

        if (subscription.Status is SubscriptionStatus.Active or SubscriptionStatus.Trialing)
        {
            await run.CallAsync<SubscriptionEntity>(
                "Subscriptions",
                "Pause subscription",
                HttpMethod.Post,
                $"subscriptions/{subscriptionId}/pause");

            await run.CallAsync<SubscriptionEntity>(
                "Subscriptions",
                "Resume subscription",
                HttpMethod.Post,
                $"subscriptions/{subscriptionId}/resume");
        }
        else
        {
            run.Skip(
                "Subscriptions",
                "Pause subscription",
                $"Subscription status is '{subscription.Status}'.");

            run.Skip(
                "Subscriptions",
                "Resume subscription",
                $"Subscription status is '{subscription.Status}'.");
        }
    }
}
