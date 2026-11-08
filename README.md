# Krema.NET

## Disclaimer

This project is an **unofficial** .NET client for the Creem API.

The library is provided **"as is"**, without warranty of any kind, express or implied. The authors and contributors accept no responsibility or liability for any damages, data loss, failed payments, unintended charges, or other consequences arising from its use. You are responsible for anything created with your API keys, especially in production mode.

## Installation

Reference the library from your project:

```powershell
dotnet add reference ..\src\Krema.NET\Krema.NET.csproj
```

Configure the API key with environment variables or a `.env` file in the repository root:

```ini
CREEM_API_KEY=creem_test_your_key_here
CREEM_MODE=test
```

`CREEM_API_KEY` is required. `CREEM_MODE` defaults to `test`; use `prod` for live payments.

## Usage

The library contains the Creem request and response models, JSON helpers and the `CreemEndpoints` path builder. It does not include an HTTP client, so the calls themselves are made with `HttpClient`.

### Set up an HTTP client

```csharp
using System.Net.Http.Json;
using Krema.NET;
using Krema.NET.Checkout;

using var http = new HttpClient
{
    BaseAddress = new Uri(CreemConfiguration.GetBaseUrl()),
};

http.DefaultRequestHeaders.Add("x-api-key", CreemConfiguration.GetApiKey());

var endpoints = new CreemEndpoints();
```

`CreemConfiguration` reads `CREEM_API_KEY` and `CREEM_MODE` from the environment or the nearest `.env` file, then selects the test or production base URL.

### Build endpoint paths

`CreemEndpoints` returns the relative path for every supported Creem endpoint. It does not send requests, so the HTTP verb, request body, authentication header and response handling stay in your code.

```csharp
var searchPath = endpoints.SearchProducts(pageNumber: 1, pageSize: 10);
// products/search?page_number=1&page_size=10

var productPath = endpoints.GetProduct("prod_xxxxx");
// products/prod_xxxxx
```

The builder never adds a leading slash, because a leading slash replaces the base path on `HttpClient`. Ids and query values are escaped with `Uri.EscapeDataString`.

| Group | Methods |
| --- | --- |
| Products | `SearchProducts`, `CreateProduct`, `GetProduct`, `UpdateProduct`, `DeleteProduct` |
| Customers | `CreateCustomer`, `UpdateCustomer`, `GetCustomerById`, `GetCustomerByEmail`, `ListCustomers`, `CreateCustomerPortalLink`, `ListCustomerOrders`, `ListCustomerLicenses`, `ListCustomerSubscriptions` |
| Checkouts | `CreateCheckout`, `GetCheckout` |
| Discounts | `CreateDiscount`, `GetDiscountById`, `GetDiscountByCode`, `SearchDiscounts`, `DeleteDiscount` |
| Stats | `GetStatsSummary` |
| Moderation | `ScreenPrompt` |
| Transactions | `SearchTransactions`, `GetTransaction` |
| Subscriptions | `SearchSubscriptions`, `GetSubscription`, `UpdateSubscription`, `PauseSubscription`, `ResumeSubscription` |
| Affiliates | `ListAffiliates`, `ListAffiliateInvites`, `GetAffiliate`, `ListAffiliateCommissions` |
| Splits | `ListSplits`, `CreateSplit`, `GetSplit`, `DeleteSplit` |
| Licenses | `ListLicenseInstances`, `ActivateLicense`, `ValidateLicense`, `DeactivateLicense` |
| Meters | `CreateMeter`, `ListMeters`, `PreviewMeter`, `GetMeter`, `UpdateMeter`, `PreviewMeter(meterId)`, `ArchiveMeter`, `UnarchiveMeter`, `GetMeterConsumedUnits` |
| Usage events | `PreviewUsageEvents`, `IngestUsageEvents`, `ListUsageEvents` |
| Customer credits | `CreateCreditAccount`, `ListCreditAccounts`, `GetCreditAccount`, `GetCreditBalance`, `CreditAccount`, `DebitAccount`, `ListCreditEntries`, `ReverseCreditAccount`, `FreezeCreditAccount`, `UnfreezeCreditAccount`, `CloseCreditAccount`, `PostCreditTransaction`, `GetCreditTransaction`, `ListCreditTransactions`, `ReverseCreditTransaction` |

### List products

```csharp
using Krema.NET.Products;

var products = await http.GetFromJsonAsync<ProductListEntity>(
    endpoints.SearchProducts(pageNumber: 1, pageSize: 10),
    CreemJson.Options);

foreach (var product in products!.Items)
{
    Console.WriteLine($"{product.Id} - {product.Name} - {product.Price} {product.Currency}");
}
```

### Create a product

```csharp
using Krema.NET.Products;

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

using var response = await http.PostAsJsonAsync(endpoints.CreateProduct(), request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var product = await response.Content.ReadFromJsonAsync<ProductEntity>(CreemJson.Options);
```

### Create a checkout

```csharp
using Krema.NET.Checkout;

var request = new CreateCheckoutRequest
{
    ProductId = "prod_xxxxx",
};

using var response = await http.PostAsJsonAsync(endpoints.CreateCheckout(), request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var checkout = await response.Content.ReadFromJsonAsync<CheckoutEntity>(CreemJson.Options);

Console.WriteLine(checkout!.CheckoutUrl); // redirect the customer here
```

`ProductId` is the only required field. `SuccessUrl` is optional; set it to your own page if you want the customer redirected after payment. Other optional fields include `Units`, `DiscountCode`, `Customer` (prefill), `Metadata` and `RequestId`.

### Create a customer

```csharp
using Krema.NET.Customers;

var request = new CreateCustomerRequestEntity
{
    Email = "customer@example.com",
    Name = "Jane Doe",
    ExternalId = "user_123",
};

using var response = await http.PostAsJsonAsync(endpoints.CreateCustomer(), request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var customer = await response.Content.ReadFromJsonAsync<CustomerEntity>(CreemJson.Options);
```

### Retrieve a customer

```csharp
using Krema.NET.Customers;

var byId = await http.GetFromJsonAsync<CustomerEntity>(
    endpoints.GetCustomerById("cust_xxxxx"),
    CreemJson.Options);

var byEmail = await http.GetFromJsonAsync<CustomerEntity>(
    endpoints.GetCustomerByEmail("customer@example.com"),
    CreemJson.Options);
```

### Open the customer portal

```csharp
using Krema.NET.Customers;

var request = new CreateCustomerPortalLinkRequestEntity
{
    CustomerId = "cust_xxxxx",
};

using var response = await http.PostAsJsonAsync(endpoints.CreateCustomerPortalLink(), request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var portal = await response.Content.ReadFromJsonAsync<CustomerLinksEntity>(CreemJson.Options);

Console.WriteLine(portal!.CustomerPortalLink);
```

### List and read subscriptions

```csharp
using Krema.NET.Subscriptions;

var subscriptions = await http.GetFromJsonAsync<SubscriptionListEntity>(
    endpoints.SearchSubscriptions(pageNumber: 1, pageSize: 10),
    CreemJson.Options);

var subscription = await http.GetFromJsonAsync<SubscriptionEntity>(
    endpoints.GetSubscription("sub_xxxxx"),
    CreemJson.Options);
```

Pause and resume are separate POST calls with no body:

```csharp
using var pause = await http.PostAsync(endpoints.PauseSubscription("sub_xxxxx"), null);
pause.EnsureSuccessStatusCode();

using var resume = await http.PostAsync(endpoints.ResumeSubscription("sub_xxxxx"), null);
resume.EnsureSuccessStatusCode();
```

### Create a discount

```csharp
using Krema.NET.Discounts;

var request = new CreateDiscountRequestEntity
{
    Name = "Launch offer",
    Code = "LAUNCH10",
    Type = DiscountEntityType.Percentage,
    Percentage = 10,
    Duration = CouponDurationType.Once,
    AppliesToProducts = new List<string> { "prod_xxxxx" },
};

using var response = await http.PostAsJsonAsync(endpoints.CreateDiscount(), request, CreemJson.Options);
response.EnsureSuccessStatusCode();

var discount = await response.Content.ReadFromJsonAsync<DiscountEntity>(CreemJson.Options);
```

Read an existing discount by code:

```csharp
var existing = await http.GetFromJsonAsync<DiscountEntity>(
    endpoints.GetDiscountByCode("LAUNCH10"),
    CreemJson.Options);
```

### Search transactions

```csharp
using Krema.NET.Transactions;

var transactions = await http.GetFromJsonAsync<TransactionListEntity>(
    endpoints.SearchTransactions(pageNumber: 1, pageSize: 10),
    CreemJson.Options);

var transaction = await http.GetFromJsonAsync<TransactionEntity>(
    endpoints.GetTransaction("tran_xxxxx"),
    CreemJson.Options);

Console.WriteLine(transactions!.Items.Count);
Console.WriteLine(transaction!.Amount);
```

### Read store stats

```csharp
using Krema.NET.Stats;

var summary = await http.GetFromJsonAsync<StatsSummaryEntity>(
    endpoints.GetStatsSummary("USD"),
    CreemJson.Options);

var monthly = await http.GetFromJsonAsync<StatsSummaryEntity>(
    endpoints.GetStatsSummary(
        "USD",
        "month",
        DateTimeOffset.UtcNow.AddDays(-30),
        DateTimeOffset.UtcNow),
    CreemJson.Options);

Console.WriteLine(summary!.Totals.TotalProducts);
Console.WriteLine(monthly!.Totals.TotalRevenue);
```

### Create a meter and send usage

```csharp
using Krema.NET.Meters;
using Krema.NET.Usage;

var meterRequest = new CreateMeterApiRequestDto
{
    Name = "API calls",
    EventName = "api_call",
    Aggregation = MeterAggregation.Count,
    UnitLabel = "call",
};

using var meterResponse = await http.PostAsJsonAsync(
    endpoints.CreateMeter(), meterRequest, CreemJson.Options);
meterResponse.EnsureSuccessStatusCode();

var meter = await meterResponse.Content.ReadFromJsonAsync<MeterApiResponseDto>(
    CreemJson.Options);

Console.WriteLine(meter!.Id);

var events = new IngestUsageEventsApiRequestDto
{
    Events = new List<IngestUsageEventApiRequestDto>
    {
        new()
        {
            Name = "api_call",
            CustomerId = "cust_xxxxx",
            EventId = "evt_0001",
        },
    },
};

using var ingestResponse = await http.PostAsJsonAsync(
    endpoints.IngestUsageEvents(), events, CreemJson.Options);
ingestResponse.EnsureSuccessStatusCode();
```

### Manage customer credits

```csharp
using Krema.NET.Accounts;

var accountRequest = new CreateAccountDto
{
    Name = "Free credits",
    CustomerId = "cust_xxxxx",
    UnitLabel = "credit",
    InitialBalance = "100",
};

using var accountResponse = await http.PostAsJsonAsync(
    endpoints.CreateCreditAccount(), accountRequest, CreemJson.Options);
accountResponse.EnsureSuccessStatusCode();

var account = await accountResponse.Content.ReadFromJsonAsync<AccountResponseDto>(
    CreemJson.Options);

var accountId = account!.Id;

var balance = await http.GetFromJsonAsync<BalanceResponseDto>(
    endpoints.GetCreditBalance(accountId),
    CreemJson.Options);

Console.WriteLine(balance!.Balance);

var creditRequest = new CreditDebitRequestDto
{
    Amount = "10",
    Reference = "goodwill-credit",
    IdempotencyKey = "goodwill-credit-1",
};

using var creditResponse = await http.PostAsJsonAsync(
    endpoints.CreditAccount(accountId), creditRequest, CreemJson.Options);
creditResponse.EnsureSuccessStatusCode();
```

### Manage affiliates, splits and licenses

```csharp
using Krema.NET.Affiliates;
using Krema.NET.Licenses;
using Krema.NET.Splits;

var affiliates = await http.GetFromJsonAsync<AffiliateListEntity>(
    endpoints.ListAffiliates(pageNumber: 1, pageSize: 5),
    CreemJson.Options);

var splits = await http.GetFromJsonAsync<SplitListEntity>(
    endpoints.ListSplits(pageNumber: 1, pageSize: 5),
    CreemJson.Options);

var licenses = await http.GetFromJsonAsync<LicenseListEntity>(
    endpoints.ListCustomerLicenses("cust_xxxxx", pageNumber: 1, pageSize: 5),
    CreemJson.Options);

Console.WriteLine(affiliates!.Items.Count);
Console.WriteLine(splits!.Items.Count);
Console.WriteLine(licenses!.Items.Count);

var activationRequest = new ActivateLicenseRequestEntity
{
    Key = "license_xxxxx",
    InstanceName = "workstation-1",
};

using var activationResponse = await http.PostAsJsonAsync(
    endpoints.ActivateLicense(), activationRequest, CreemJson.Options);
activationResponse.EnsureSuccessStatusCode();
```

### Screen a prompt

```csharp
using Krema.NET.Moderation;

var promptRequest = new ScreenPromptRequest
{
    Prompt = "Hello, this is a test.",
    ExternalId = "moderation-1",
};

using var screenResponse = await http.PostAsJsonAsync(
    endpoints.ScreenPrompt(), promptRequest, CreemJson.Options);
screenResponse.EnsureSuccessStatusCode();

var screening = await screenResponse.Content.ReadFromJsonAsync<ScreenPromptResponse>(
    CreemJson.Options);

Console.WriteLine(screening!.Decision);
```

### Receive webhooks

`WebhookEventEntity` resolves the concrete event type from the `eventType` field:

```csharp
using System.Text.Json;
using Krema.NET.Checkout;
using Krema.NET.Webhooks;

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

- Endpoint paths come from `CreemEndpoints`. Add a method there instead of hardcoding a path in caller code.
- Money values are `decimal`.
- Enums serialize to the exact strings the API expects.
- `CreemJson.Options` omits null members, so optional fields are not sent.
- `string | object` union fields are `JsonElement` properties. Read them with `CreemJson.GetId(...)` or `CreemJson.GetObject<T>(...)`.
- API errors are not modeled. Check `IsSuccessStatusCode` and read the response body when a call fails.
- Working examples for every endpoint are in `test/Krema.NET.ApiTests/Scenarios`.
