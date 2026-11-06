using System.Net.Http;
using KremaDotNet.Affiliates;
using KremaDotNet.Checkout;
using KremaDotNet.Licenses;
using KremaDotNet.Splits;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Runs the platform scenarios: affiliates, revenue splits and license keys.
/// </summary>
internal static class PlatformScenarios
{
    /// <summary>Runs all platform scenarios in order.</summary>
    public static async Task RunAsync(TestRun run)
    {
        await AffiliatesAsync(run);
        await SplitsAsync(run);
        await LicensesAsync(run);
    }

    /// <summary>Lists affiliates and invites, then reads an affiliate and its commissions when one exists.</summary>
    private static async Task AffiliatesAsync(TestRun run)
    {
        var affiliates = await run.CallAsync<AffiliateListEntity>(
            "Affiliates",
            "List affiliates",
            HttpMethod.Get,
            "affiliates?page_number=1&page_size=5");

        await run.CallAsync<AffiliateInviteListEntity>(
            "Affiliates",
            "List invites",
            HttpMethod.Get,
            "affiliates/invites?page_number=1&page_size=5");

        run.AffiliateId = affiliates?.Items.FirstOrDefault()?.Id;

        if (run.AffiliateId is null)
        {
            run.Skip("Affiliates", "Retrieve affiliate", "No affiliate exists in the test account.");
            run.Skip("Affiliates", "List commissions", "No affiliate exists in the test account.");
            return;
        }

        var affiliateId = Uri.EscapeDataString(run.AffiliateId);

        await run.CallAsync<AffiliateEntity>(
            "Affiliates",
            "Retrieve affiliate",
            HttpMethod.Get,
            $"affiliates/{affiliateId}");

        await run.CallAsync<CommissionListEntity>(
            "Affiliates",
            "List commissions",
            HttpMethod.Get,
            $"affiliates/{affiliateId}/commissions?page_number=1&page_size=5");
    }

    /// <summary>Lists splits, then creates a product split and reads it back.</summary>
    private static async Task SplitsAsync(TestRun run)
    {
        var productId = run.ProductId ?? run.ExistingProductId;

        await run.CallAsync<SplitListEntity>(
            "Splits",
            "List splits",
            HttpMethod.Get,
            "splits?page_number=1&page_size=5");

        if (productId is null)
        {
            run.Skip("Splits", "Create split", "No product available.");
            run.Skip("Splits", "Get split", "No product available.");
            return;
        }

        var split = await run.CallAsync<SplitEntity>(
            "Splits",
            "Create split",
            HttpMethod.Post,
            "splits",
            new CreateSplitRequestEntity
            {
                Description = $"Krema Test Split {run.RunId}",
                Type = SplitEntityType.Product,
                TypeReference = productId,
                Recipients = new List<CreateSplitRecipientRequestEntity>
                {
                    new()
                    {
                        RecipientType = SplitRecipientRequestKind.Email,
                        RecipientReference = $"split+{run.RunId}@example.com",
                        Amount = 10,
                    },
                },
            });

        run.SplitId = split?.Id;

        if (run.SplitId is null)
        {
            run.Skip("Splits", "Get split", "Split creation failed.");
            return;
        }

        await run.CallAsync<SplitEntity>(
            "Splits",
            "Get split",
            HttpMethod.Get,
            $"splits/{Uri.EscapeDataString(run.SplitId)}");
    }

    /// <summary>Lists the customer's license keys and, when one exists, activates, validates and deactivates it.</summary>
    private static async Task LicensesAsync(TestRun run)
    {
        if (run.CustomerId is null)
        {
            run.Skip("Licenses", "List customer licenses", "No customer available.");
            run.Skip("Licenses", "List license instances", "No customer available.");
            run.Skip("Licenses", "Activate license", "No customer available.");
            run.Skip("Licenses", "Validate license", "No customer available.");
            run.Skip("Licenses", "Deactivate license", "No customer available.");
            return;
        }

        var licenses = await run.CallAsync<LicenseListEntity>(
            "Licenses",
            "List customer licenses",
            HttpMethod.Get,
            $"customers/{Uri.EscapeDataString(run.CustomerId)}/licenses?page_number=1&page_size=10");

        var license = licenses?.Items.FirstOrDefault();

        if (license is null)
        {
            run.Skip("Licenses", "List license instances", "The test customer has no license keys.");
            run.Skip("Licenses", "Activate license", "The test customer has no license keys.");
            run.Skip("Licenses", "Validate license", "The test customer has no license keys.");
            run.Skip("Licenses", "Deactivate license", "The test customer has no license keys.");
            return;
        }

        run.LicenseId = license.Id;
        run.LicenseKey = license.Key;

        await run.CallAsync<LicenseInstanceListEntity>(
            "Licenses",
            "List license instances",
            HttpMethod.Get,
            $"licenses/{Uri.EscapeDataString(run.LicenseId)}/instances?page_number=1&page_size=10");

        var activated = await run.CallAsync<LicenseEntity>(
            "Licenses",
            "Activate license",
            HttpMethod.Post,
            "licenses/activate",
            new ActivateLicenseRequestEntity
            {
                Key = run.LicenseKey,
                InstanceName = $"krema-{run.RunId}",
            });

        var instanceId = activated?.Instance?.Id;

        if (instanceId is null)
        {
            run.Skip("Licenses", "Validate license", "License activation did not return an instance.");
            run.Skip("Licenses", "Deactivate license", "License activation did not return an instance.");
            return;
        }

        await run.CallAsync<LicenseEntity>(
            "Licenses",
            "Validate license",
            HttpMethod.Post,
            "licenses/validate",
            new ValidateLicenseRequestEntity
            {
                Key = run.LicenseKey,
                InstanceId = instanceId,
            });

        await run.CallAsync<LicenseEntity>(
            "Licenses",
            "Deactivate license",
            HttpMethod.Post,
            "licenses/deactivate",
            new DeactivateLicenseRequestEntity
            {
                Key = run.LicenseKey,
                InstanceId = instanceId,
            });
    }
}
