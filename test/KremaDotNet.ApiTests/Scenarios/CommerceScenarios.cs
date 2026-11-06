using System.Net.Http;
using KremaDotNet.Checkout;
using KremaDotNet.Customers;
using KremaDotNet.Discounts;
using KremaDotNet.Licenses;
using KremaDotNet.Moderation;
using KremaDotNet.Orders;
using KremaDotNet.Products;
using KremaDotNet.Stats;
using KremaDotNet.Subscriptions;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Runs the commerce scenarios: products, customers, checkouts, discounts, stats and moderation.
/// </summary>
internal static class CommerceScenarios
{
    /// <summary>Runs all commerce scenarios in order.</summary>
    public static async Task RunAsync(TestRun run)
    {
        await ProductsAsync(run);
        await CustomersAsync(run);
        await CheckoutsAsync(run);
        await DiscountsAsync(run);
        await StatsAsync(run);
        await ModerationAsync(run);
    }

    /// <summary>Searches existing products, then creates, reads and updates one-time and recurring products.</summary>
    private static async Task ProductsAsync(TestRun run)
    {
        var existing = await run.CallAsync<ProductListEntity>(
            "Products",
            "Search products",
            HttpMethod.Get,
            "products/search?page_number=1&page_size=5");

        if (existing?.Items is { Count: > 0 })
        {
            run.ExistingProductId = existing.Items[0].Id;
        }

        var product = await run.CallAsync<ProductEntity>(
            "Products",
            "Create one-time product",
            HttpMethod.Post,
            "products",
            new CreateProductRequestEntity
            {
                Name = $"KremaDotNET Test Product {run.RunId}",
                Description = "Product created by the KremaDotNET API coverage runner.",
                Price = 100,
                Currency = ProductCurrency.Usd,
                BillingType = ProductRequestBillingType.Onetime,
                TaxMode = TaxMode.Inclusive,
                TaxCategory = TaxCategory.Saas,
            });

        run.ProductId = product?.Id;

        var recurring = await run.CallAsync<ProductEntity>(
            "Products",
            "Create recurring product",
            HttpMethod.Post,
            "products",
            new CreateProductRequestEntity
            {
                Name = $"KremaDotNET Test Subscription {run.RunId}",
                Description = "Recurring product created by the KremaDotNET API coverage runner.",
                Price = 100,
                Currency = ProductCurrency.Usd,
                BillingType = ProductRequestBillingType.Recurring,
                BillingPeriod = ProductRequestBillingPeriod.EveryMonth,
                TaxMode = TaxMode.Inclusive,
                TaxCategory = TaxCategory.Saas,
            });

        run.RecurringProductId = recurring?.Id;

        if (run.ProductId is null)
        {
            run.Skip("Products", "Get product", "Product creation failed.");
            run.Skip("Products", "Update product", "Product creation failed.");
            return;
        }

        await run.CallAsync<ProductEntity>(
            "Products",
            "Get product",
            HttpMethod.Get,
            $"products/{Uri.EscapeDataString(run.ProductId)}");

        await run.CallAsync<ProductEntity>(
            "Products",
            "Update product",
            HttpMethod.Patch,
            $"products/{Uri.EscapeDataString(run.ProductId)}",
            new UpdateProductRequestEntity
            {
                Description = "Updated by the KremaDotNET API coverage runner.",
            });
    }

    /// <summary>Creates a customer, then reads, updates, lists and fetches its billing link, orders, licenses and subscriptions.</summary>
    private static async Task CustomersAsync(TestRun run)
    {
        var email = $"krema+{run.RunId}@example.com";

        var customer = await run.CallAsync<CustomerEntity>(
            "Customers",
            "Create customer",
            HttpMethod.Post,
            "customers",
            new CreateCustomerRequestEntity
            {
                Email = email,
                Name = $"Krema Test Customer {run.RunId}",
                ExternalId = $"krema-{run.RunId}",
            });

        run.CustomerId = customer?.Id;
        run.CustomerEmail = customer?.Email ?? email;

        if (run.CustomerId is null)
        {
            run.Skip("Customers", "Retrieve customer by id", "Customer creation failed.");
            run.Skip("Customers", "Retrieve customer by email", "Customer creation failed.");
            run.Skip("Customers", "Update customer", "Customer creation failed.");
            run.Skip("Customers", "Create customer portal link", "Customer creation failed.");
            run.Skip("Customers", "List customer orders", "Customer creation failed.");
            run.Skip("Customers", "List customer licenses", "Customer creation failed.");
            run.Skip("Customers", "List customer subscriptions", "Customer creation failed.");
            return;
        }

        var customerId = Uri.EscapeDataString(run.CustomerId);

        await run.CallAsync<CustomerEntity>(
            "Customers",
            "Retrieve customer by id",
            HttpMethod.Get,
            $"customers?customer_id={customerId}");

        await run.CallAsync<CustomerEntity>(
            "Customers",
            "Retrieve customer by email",
            HttpMethod.Get,
            $"customers?email={Uri.EscapeDataString(run.CustomerEmail)}");

        await run.CallAsync<CustomerEntity>(
            "Customers",
            "Update customer",
            HttpMethod.Patch,
            "customers",
            new UpdateCustomerRequestEntity
            {
                CustomerId = run.CustomerId,
                Name = $"Krema Test Customer {run.RunId} Updated",
            });

        await run.CallAsync<CustomerListEntity>(
            "Customers",
            "List customers",
            HttpMethod.Get,
            "customers/list?page_number=1&page_size=5");

        await run.CallAsync<CustomerLinksEntity>(
            "Customers",
            "Create customer portal link",
            HttpMethod.Post,
            "customers/billing",
            new CreateCustomerPortalLinkRequestEntity
            {
                CustomerId = run.CustomerId,
            });

        await run.CallAsync<OrderListEntity>(
            "Customers",
            "List customer orders",
            HttpMethod.Get,
            $"customers/{customerId}/orders?page_number=1&page_size=5");

        await run.CallAsync<LicenseListEntity>(
            "Customers",
            "List customer licenses",
            HttpMethod.Get,
            $"customers/{customerId}/licenses?page_number=1&page_size=5");

        await run.CallAsync<SubscriptionListEntity>(
            "Customers",
            "List customer subscriptions",
            HttpMethod.Get,
            $"customers/{customerId}/subscriptions?page_number=1&page_size=5");
    }

    /// <summary>Creates a checkout session for the customer and retrieves it.</summary>
    private static async Task CheckoutsAsync(TestRun run)
    {
        var productId = run.ProductId ?? run.ExistingProductId;

        if (productId is null || run.CustomerId is null)
        {
            run.Skip("Checkouts", "Create checkout", "No product or customer available.");
            run.Skip("Checkouts", "Retrieve checkout", "No product or customer available.");
            return;
        }

        var checkout = await run.CallAsync<CheckoutEntity>(
            "Checkouts",
            "Create checkout",
            HttpMethod.Post,
            "checkouts",
            new CreateCheckoutRequest
            {
                ProductId = productId,
                Units = 1,
                Customer = new CustomerRequestEntity
                {
                    Id = run.CustomerId,
                },
                SuccessUrl = "https://example.com/creem-test-success",
            });

        run.CheckoutId = checkout?.Id;

        if (run.CheckoutId is null)
        {
            run.Skip("Checkouts", "Retrieve checkout", "Checkout creation failed.");
            return;
        }

        await run.CallAsync<CheckoutEntity>(
            "Checkouts",
            "Retrieve checkout",
            HttpMethod.Get,
            $"checkouts?checkout_id={Uri.EscapeDataString(run.CheckoutId)}");
    }

    /// <summary>Creates a percentage discount, then reads it by id and code and searches discounts.</summary>
    private static async Task DiscountsAsync(TestRun run)
    {
        var productId = run.ProductId ?? run.ExistingProductId;

        if (productId is null)
        {
            run.Skip("Discounts", "Create discount", "No product available.");
            run.Skip("Discounts", "Get discount by id", "No product available.");
            run.Skip("Discounts", "Get discount by code", "No product available.");
            run.Skip("Discounts", "Search discounts", "No product available.");
            return;
        }

        var discount = await run.CallAsync<DiscountEntity>(
            "Discounts",
            "Create discount",
            HttpMethod.Post,
            "discounts",
            new CreateDiscountRequestEntity
            {
                Name = $"Krema Test Discount {run.RunId}",
                Code = $"KREMA{run.RunId[^8..]}",
                Type = DiscountEntityType.Percentage,
                Percentage = 10,
                Duration = CouponDurationType.Once,
                AppliesToProducts = new List<string> { productId },
            });

        run.DiscountId = discount?.Id;
        run.DiscountCode = discount?.Code;

        if (run.DiscountId is null)
        {
            run.Skip("Discounts", "Get discount by id", "Discount creation failed.");
            run.Skip("Discounts", "Get discount by code", "Discount creation failed.");
            run.Skip("Discounts", "Search discounts", "Discount creation failed.");
            return;
        }

        await run.CallAsync<DiscountEntity>(
            "Discounts",
            "Get discount by id",
            HttpMethod.Get,
            $"discounts?discount_id={Uri.EscapeDataString(run.DiscountId)}");

        await run.CallAsync<DiscountEntity>(
            "Discounts",
            "Get discount by code",
            HttpMethod.Get,
            $"discounts?discount_code={Uri.EscapeDataString(run.DiscountCode!)}");

        await run.CallAsync<DiscountListEntity>(
            "Discounts",
            "Search discounts",
            HttpMethod.Get,
            "discounts/search?page_number=1&page_size=5");
    }

    /// <summary>Reads the store stats summary with and without a time interval.</summary>
    private static async Task StatsAsync(TestRun run)
    {
        await run.CallAsync<StatsSummaryEntity>(
            "Stats",
            "Get summary",
            HttpMethod.Get,
            "stats/summary?currency=USD");

        var end = DateTimeOffset.UtcNow;
        var start = end.AddDays(-30);
        var path =
            $"stats/summary?currency=USD&interval=month" +
            $"&startDate={start.ToUnixTimeMilliseconds()}" +
            $"&endDate={end.ToUnixTimeMilliseconds()}";

        await run.CallAsync<StatsSummaryEntity>(
            "Stats",
            "Get summary with interval",
            HttpMethod.Get,
            path);
    }

    /// <summary>Screens a test prompt through the moderation endpoint.</summary>
    private static async Task ModerationAsync(TestRun run)
    {
        await run.CallAsync<ScreenPromptResponse>(
            "Moderation",
            "Screen prompt",
            HttpMethod.Post,
            "moderation/prompt",
            new ScreenPromptRequest
            {
                Prompt = "Hello, this is a KremaDotNET API coverage test.",
                ExternalId = $"krema-{run.RunId}",
            });
    }
}
