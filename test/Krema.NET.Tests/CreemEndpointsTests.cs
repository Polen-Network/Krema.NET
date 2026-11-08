using Krema.NET;

namespace Krema.NET.Tests;

public class CreemEndpointsTests
{
    private readonly CreemEndpoints endpoints = new();

    [Fact]
    public void SearchProducts_UsesDefaults()
    {
        Assert.Equal(
            "products/search?page_number=1&page_size=10",
            endpoints.SearchProducts());
    }

    [Fact]
    public void SearchProducts_UsesGivenPagination()
    {
        Assert.Equal(
            "products/search?page_number=2&page_size=25",
            endpoints.SearchProducts(2, 25));
    }

    [Fact]
    public void ProductPaths_EscapeIds()
    {
        const string expected = "products/prod%2F1%20x";

        Assert.Equal(expected, endpoints.GetProduct("prod/1 x"));
        Assert.Equal(expected, endpoints.UpdateProduct("prod/1 x"));
        Assert.Equal(expected, endpoints.DeleteProduct("prod/1 x"));
    }

    [Fact]
    public void CustomerLookups_EscapeQueryValues()
    {
        Assert.Equal(
            "customers?customer_id=cust%2F1",
            endpoints.GetCustomerById("cust/1"));

        Assert.Equal(
            "customers?email=a%2Bb%40example.com",
            endpoints.GetCustomerByEmail("a+b@example.com"));
    }

    [Fact]
    public void CustomerSubresources_IncludePagination()
    {
        Assert.Equal(
            "customers/cust%2F1/orders?page_number=1&page_size=5",
            endpoints.ListCustomerOrders("cust/1", 1, 5));

        Assert.Equal(
            "customers/cust%2F1/licenses?page_number=1&page_size=5",
            endpoints.ListCustomerLicenses("cust/1", 1, 5));

        Assert.Equal(
            "customers/cust%2F1/subscriptions?page_number=1&page_size=5",
            endpoints.ListCustomerSubscriptions("cust/1", 1, 5));
    }

    [Fact]
    public void StatsSummary_FormatsDatesAsUnixMilliseconds()
    {
        var start = DateTimeOffset.FromUnixTimeMilliseconds(0);
        var end = DateTimeOffset.FromUnixTimeMilliseconds(86_400_000);

        Assert.Equal(
            "stats/summary?currency=USD&interval=month&startDate=0&endDate=86400000",
            endpoints.GetStatsSummary("USD", "month", start, end));
    }

    [Fact]
    public void CreditPaths_UseExpectedSegments()
    {
        Assert.Equal(
            "customer-credits/accounts/acct%2F1/balance",
            endpoints.GetCreditBalance("acct/1"));

        Assert.Equal(
            "customer-credits/accounts/acct%2F1/entries?limit=20",
            endpoints.ListCreditEntries("acct/1", 20));

        Assert.Equal(
            "customer-credits/transactions?reference=ref%2F1",
            endpoints.ListCreditTransactions("ref/1"));
    }

    [Fact]
    public void Paths_AreRelative()
    {
        AssertRelative(endpoints.CreateProduct());
        AssertRelative(endpoints.CreateCustomer());
        AssertRelative(endpoints.CreateCheckout());
        AssertRelative(endpoints.CreateDiscount());
        AssertRelative(endpoints.ScreenPrompt());
        AssertRelative(endpoints.CreateSplit());
        AssertRelative(endpoints.ActivateLicense());
        AssertRelative(endpoints.CreateMeter());
        AssertRelative(endpoints.IngestUsageEvents());
        AssertRelative(endpoints.CreateCreditAccount());
    }

    private static void AssertRelative(string path)
    {
        Assert.False(path.StartsWith("/", StringComparison.Ordinal), $"Path starts with '/': {path}");
    }
}
