using System.Net.Http.Json;
using Krema.NET.Checkout;
using Krema.NET.Products;

namespace Krema.NET.Tests;

public class ApiTests
{
    [Fact]
    public async Task ListProducts_AgainstTestMode()
    {
        string apiKey;

        try
        {
            apiKey = CreemConfiguration.GetApiKey();
        }
        catch (InvalidOperationException)
        {
          
            return;
        }

        if (CreemConfiguration.GetMode() != "test")
        {
            return;
        }

        using var http = new HttpClient
        {
            BaseAddress = new Uri(CreemConfiguration.GetBaseUrl()),
        };

        http.DefaultRequestHeaders.Add("x-api-key", apiKey);

        using var response = await http.GetAsync("products/search");
        response.EnsureSuccessStatusCode();

        var page = await response.Content.ReadFromJsonAsync<ProductListEntity>(CreemJson.Options);

        Assert.NotNull(page);
        Assert.NotNull(page!.Items);
        Assert.NotNull(page.Pagination);
    }
}
