# KremaDotNet

## Disclaimer

This project is an **unofficial** .NET client for the Creem API.

The library is provided **"as is"**, without warranty of any kind, express or implied. The authors and contributors accept no responsibility or liability for any damages, data loss, failed payments, unintended charges, or other consequences arising from its use. You are responsible for anything created with your API keys, especially in production mode.

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

The library contains the Creem request and response models plus a few JSON helpers. It does not include an HTTP client, so the calls themselves are made with `HttpClient`.

### Set up an HTTP client

```csharp
using System.Net.Http.Json;
using KremaDotNet.Checkout;

using var http = new HttpClient
{
    BaseAddress = new Uri(CreemConfiguration.GetBaseUrl()),
};

http.DefaultRequestHeaders.Add("x-api-key", CreemConfiguration.GetApiKey());
```

`CreemConfiguration` reads `CREEM_API_KEY` and `CREEM_MODE` from the environment or the nearest `.env` file, then selects the test or production base URL.

### List products

```csharp
using KremaDotNet.Products;

var products = await http.GetFromJsonAsync<ProductListEntity>(
    "products/search?page_number=1&page_size=10",
    CreemJson.Options);

foreach (var product in products!.Items)
{
    Console.WriteLine($"{product.Id} - {product.Name} - {product.Price} {product.Currency}");
}
```

### Create a product

```csharp
using KremaDotNet.Products;

var request = new CreateProductRequestEntity
{
    Name = "Pro Plan",
    Description = "Monthly subscription",
    Price = 19.99m,
    Currency = ProductCurrency.Usd,
    BillingType = ProductRequestBillingType.Recurring,
    BillingPeriod = ProductRequestBillingPeriod.EveryMonth,
    TaxMode = TaxMode.Inclusive,
};

using var response = await http.PostAsJsonAsync("products", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var product = await response.Content.ReadFromJsonAsync<ProductEntity>(CreemJson.Options);
```

### Create a checkout

```csharp
using KremaDotNet.Checkout;

var request = new CreateCheckoutRequest
{
    ProductId = "prod_xxxxx",
};

using var response = await http.PostAsJsonAsync("checkouts", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var checkout = await response.Content.ReadFromJsonAsync<CheckoutEntity>(CreemJson.Options);

Console.WriteLine(checkout!.CheckoutUrl); // redirect the customer here
```

`ProductId` is the only required field. `SuccessUrl` is optional; set it to your own page if you want the customer redirected after payment. Other optional fields include `Units`, `DiscountCode`, `Customer` (prefill), `Metadata` and `RequestId`.

### Create a customer

```csharp
using KremaDotNet.Customers;

var request = new CreateCustomerRequestEntity
{
    Email = "customer@example.com",
    Name = "Jane Doe",
    ExternalId = "user_123",
};

using var response = await http.PostAsJsonAsync("customers", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var customer = await response.Content.ReadFromJsonAsync<CustomerEntity>(CreemJson.Options);
```

### Retrieve a customer

```csharp
using KremaDotNet.Customers;

var byId = await http.GetFromJsonAsync<CustomerEntity>(
    $"customers?customer_id={Uri.EscapeDataString("cust_xxxxx")}",
    CreemJson.Options);

var byEmail = await http.GetFromJsonAsync<CustomerEntity>(
    $"customers?email={Uri.EscapeDataString("customer@example.com")}",
    CreemJson.Options);
```

### Open the customer portal

```csharp
using KremaDotNet.Customers;

var request = new CreateCustomerPortalLinkRequestEntity
{
    CustomerId = "cust_xxxxx",
};

using var response = await http.PostAsJsonAsync("customers/billing", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var portal = await response.Content.ReadFromJsonAsync<CustomerLinksEntity>(CreemJson.Options);

Console.WriteLine(portal!.CustomerPortalLink);
```

### List and read subscriptions

```csharp
using KremaDotNet.Subscriptions;

var subscriptions = await http.GetFromJsonAsync<SubscriptionListEntity>(
    "subscriptions/search?page_number=1&page_size=10",
    CreemJson.Options);

var subscription = await http.GetFromJsonAsync<SubscriptionEntity>(
    $"subscriptions?subscription_id={Uri.EscapeDataString("sub_xxxxx")}",
    CreemJson.Options);
```

Pause and resume are separate POST calls with no body:

```csharp
var subscriptionId = Uri.EscapeDataString("sub_xxxxx");

using var pause = await http.PostAsync($"subscriptions/{subscriptionId}/pause", null);
pause.EnsureSuccessStatusCode();

using var resume = await http.PostAsync($"subscriptions/{subscriptionId}/resume", null);
resume.EnsureSuccessStatusCode();
```

### Create a discount

```csharp
using KremaDotNet.Discounts;

var request = new CreateDiscountRequestEntity
{
    Name = "Launch offer",
    Code = "LAUNCH10",
    Type = DiscountEntityType.Percentage,
    Percentage = 10,
    Duration = CouponDurationType.Once,
    AppliesToProducts = new List<string> { "prod_xxxxx" },
};

using var response = await http.PostAsJsonAsync("discounts", request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var discount = await response.Content.ReadFromJsonAsync<DiscountEntity>(CreemJson.Options);
```

Read an existing discount by code:

```csharp
var existing = await http.GetFromJsonAsync<DiscountEntity>(
    $"discounts?discount_code={Uri.EscapeDataString("LAUNCH10")}",
    CreemJson.Options);
```

### Receive webhooks

`WebhookEventEntity` resolves the concrete event type from the `eventType` field:

```csharp
using System.Text.Json;
using KremaDotNet.Checkout;
using KremaDotNet.Webhooks;

var webhookEvent = JsonSerializer.Deserialize<WebhookEventEntity>(json, CreemJson.Options);

switch (webhookEvent)
{
    case WebhookCheckoutCompletedEventEntity completed:
        Console.WriteLine($"Checkout completed: {completed.Object.Id}");
        break;
    case WebhookSubscriptionActiveEventEntity active:
        Console.WriteLine($"Subscription active: {active.Object.Id}");
        break;
}
```

Signature verification is not built in and remains the caller's responsibility.

### Notes

- Money values are `decimal`.
- Enums serialize to the exact strings the API expects.
- `CreemJson.Options` omits null members, so optional fields are not sent.
- `string | object` union fields are `JsonElement` properties. Read them with `CreemJson.GetId(...)` or `CreemJson.GetObject<T>(...)`.
- API errors are not modeled. Check `IsSuccessStatusCode` and read the response body when a call fails.
- Working examples for every endpoint are in `test/KremaDotNet.ApiTests/Scenarios`.
