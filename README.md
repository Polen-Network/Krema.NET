# KremaDotNet

## Installation

Reference the library from your project:

```powershell
dotnet add reference ..\src\KremaDotNet\KremaDotNet.csproj
```

Configure the API key with environment variables or a `.env` file in the repository root:

```ini
CREEM_API_KEY=creem_test_your_key_here
CREEM_MODE=test
```

`CREEM_API_KEY` is required. `CREEM_MODE` defaults to `test`; use `prod` for live payments.

## Usage

```csharp
using System.Net.Http.Json;
using KremaDotNet.Checkout;

using var http = new HttpClient { BaseAddress = new Uri(CreemConfiguration.GetBaseUrl()) };
http.DefaultRequestHeaders.Add("x-api-key", CreemConfiguration.GetApiKey());

// List products
var productsJson = await http.GetStringAsync("products/search");

// Create a checkout
var request = new CreateCheckoutRequest
{
    ProductId = "prod_xxxxx",
    SuccessUrl = "https://example.com/success",
};

using var response = await http.PostAsJsonAsync("checkouts", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var checkout = await response.Content.ReadFromJsonAsync<CheckoutEntity>(CreemJson.Options);

Console.WriteLine(checkout!.CheckoutUrl); // redirect the customer here
```
