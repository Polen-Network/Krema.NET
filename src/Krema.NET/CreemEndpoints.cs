using System;

namespace Krema.NET;

/// <summary>
/// Builds the relative Creem API paths that are passed to an <see cref="System.Net.Http.HttpClient"/>.
/// The base URL stays on <see cref="System.Net.Http.HttpClient.BaseAddress"/> and every method returns
/// a path relative to it, so the paths never start with a slash.
/// </summary>
/// <remarks>
/// The class only produces path strings. It does not send requests, handle authentication or
/// deserialize responses; those stay at the call site.
/// </remarks>
public sealed class CreemEndpoints
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;

    // Products

    /// <summary>Searches products with pagination.</summary>
    public string SearchProducts(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("products/search", pageNumber, pageSize);

    /// <summary>Creates a product. Use POST with this path.</summary>
    public string CreateProduct() => "products";

    /// <summary>Reads, updates (PATCH) or deletes (DELETE) one product.</summary>
    public string Product(string productId) => $"products/{Escape(productId)}";

    /// <summary>Reads one product.</summary>
    public string GetProduct(string productId) => Product(productId);

    /// <summary>Updates one product with PATCH.</summary>
    public string UpdateProduct(string productId) => Product(productId);

    /// <summary>Deletes (archives) one product with DELETE.</summary>
    public string DeleteProduct(string productId) => Product(productId);

    // Customers

    /// <summary>Creates a customer. Use POST with this path.</summary>
    public string CreateCustomer() => "customers";

    /// <summary>Updates a customer with PATCH.</summary>
    public string UpdateCustomer() => "customers";

    /// <summary>Retrieves a customer by id.</summary>
    public string GetCustomerById(string customerId) =>
        $"customers?customer_id={Escape(customerId)}";

    /// <summary>Retrieves a customer by email address.</summary>
    public string GetCustomerByEmail(string email) =>
        $"customers?email={Escape(email)}";

    /// <summary>Lists customers with pagination.</summary>
    public string ListCustomers(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("customers/list", pageNumber, pageSize);

    /// <summary>Creates a customer portal link. Use POST with this path.</summary>
    public string CreateCustomerPortalLink() => "customers/billing";

    /// <summary>Lists the orders of one customer.</summary>
    public string ListCustomerOrders(
        string customerId,
        int pageNumber = DefaultPageNumber,
        int pageSize = DefaultPageSize) =>
        Paged($"customers/{Escape(customerId)}/orders", pageNumber, pageSize);

    /// <summary>Lists the license keys of one customer.</summary>
    public string ListCustomerLicenses(
        string customerId,
        int pageNumber = DefaultPageNumber,
        int pageSize = DefaultPageSize) =>
        Paged($"customers/{Escape(customerId)}/licenses", pageNumber, pageSize);

    /// <summary>Lists the subscriptions of one customer.</summary>
    public string ListCustomerSubscriptions(
        string customerId,
        int pageNumber = DefaultPageNumber,
        int pageSize = DefaultPageSize) =>
        Paged($"customers/{Escape(customerId)}/subscriptions", pageNumber, pageSize);

    // Checkouts

    /// <summary>Creates a checkout session. Use POST with this path.</summary>
    public string CreateCheckout() => "checkouts";

    /// <summary>Retrieves a checkout session by id.</summary>
    public string GetCheckout(string checkoutId) =>
        $"checkouts?checkout_id={Escape(checkoutId)}";

    // Discounts

    /// <summary>Creates a discount. Use POST with this path.</summary>
    public string CreateDiscount() => "discounts";

    /// <summary>Retrieves a discount by id.</summary>
    public string GetDiscountById(string discountId) =>
        $"discounts?discount_id={Escape(discountId)}";

    /// <summary>Retrieves a discount by code.</summary>
    public string GetDiscountByCode(string discountCode) =>
        $"discounts?discount_code={Escape(discountCode)}";

    /// <summary>Searches discounts with pagination.</summary>
    public string SearchDiscounts(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("discounts/search", pageNumber, pageSize);

    /// <summary>Deletes one discount.</summary>
    public string DeleteDiscount(string discountId) =>
        $"discounts/{Escape(discountId)}/delete";

    // Stats

    /// <summary>Reads the store summary for one currency.</summary>
    public string GetStatsSummary(string currency) =>
        $"stats/summary?currency={Escape(currency)}";

    /// <summary>Reads the store summary for one currency and a date interval.</summary>
    public string GetStatsSummary(
        string currency,
        string interval,
        DateTimeOffset startDate,
        DateTimeOffset endDate) =>
        $"stats/summary?currency={Escape(currency)}&interval={Escape(interval)}" +
        $"&startDate={startDate.ToUnixTimeMilliseconds()}" +
        $"&endDate={endDate.ToUnixTimeMilliseconds()}";

    // Moderation

    /// <summary>Screens a prompt. Use POST with this path.</summary>
    public string ScreenPrompt() => "moderation/prompt";

    // Transactions

    /// <summary>Searches transactions with pagination.</summary>
    public string SearchTransactions(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("transactions/search", pageNumber, pageSize);

    /// <summary>Retrieves a transaction by id.</summary>
    public string GetTransaction(string transactionId) =>
        $"transactions?transaction_id={Escape(transactionId)}";

    // Subscriptions

    /// <summary>Searches subscriptions with pagination.</summary>
    public string SearchSubscriptions(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("subscriptions/search", pageNumber, pageSize);

    /// <summary>Retrieves a subscription by id.</summary>
    public string GetSubscription(string subscriptionId) =>
        $"subscriptions?subscription_id={Escape(subscriptionId)}";

    /// <summary>Updates a subscription with POST.</summary>
    public string UpdateSubscription(string subscriptionId) =>
        $"subscriptions/{Escape(subscriptionId)}";

    /// <summary>Pauses a subscription. Use POST with this path.</summary>
    public string PauseSubscription(string subscriptionId) =>
        $"subscriptions/{Escape(subscriptionId)}/pause";

    /// <summary>Resumes a subscription. Use POST with this path.</summary>
    public string ResumeSubscription(string subscriptionId) =>
        $"subscriptions/{Escape(subscriptionId)}/resume";

    // Affiliates

    /// <summary>Lists affiliates with pagination.</summary>
    public string ListAffiliates(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("affiliates", pageNumber, pageSize);

    /// <summary>Lists affiliate invites with pagination.</summary>
    public string ListAffiliateInvites(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("affiliates/invites", pageNumber, pageSize);

    /// <summary>Retrieves one affiliate.</summary>
    public string GetAffiliate(string affiliateId) =>
        $"affiliates/{Escape(affiliateId)}";

    /// <summary>Lists the commissions of one affiliate.</summary>
    public string ListAffiliateCommissions(
        string affiliateId,
        int pageNumber = DefaultPageNumber,
        int pageSize = DefaultPageSize) =>
        Paged($"affiliates/{Escape(affiliateId)}/commissions", pageNumber, pageSize);

    // Splits

    /// <summary>Lists revenue splits with pagination.</summary>
    public string ListSplits(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize) =>
        Paged("splits", pageNumber, pageSize);

    /// <summary>Creates a revenue split. Use POST with this path.</summary>
    public string CreateSplit() => "splits";

    /// <summary>Reads or deletes one revenue split.</summary>
    public string Split(string splitId) => $"splits/{Escape(splitId)}";

    /// <summary>Reads one revenue split.</summary>
    public string GetSplit(string splitId) => Split(splitId);

    /// <summary>Deletes one revenue split with DELETE.</summary>
    public string DeleteSplit(string splitId) => Split(splitId);

    // Licenses

    /// <summary>Lists the instances of one license key.</summary>
    public string ListLicenseInstances(
        string licenseId,
        int pageNumber = DefaultPageNumber,
        int pageSize = DefaultPageSize) =>
        Paged($"licenses/{Escape(licenseId)}/instances", pageNumber, pageSize);

    /// <summary>Activates a license key. Use POST with this path.</summary>
    public string ActivateLicense() => "licenses/activate";

    /// <summary>Validates a license key. Use POST with this path.</summary>
    public string ValidateLicense() => "licenses/validate";

    /// <summary>Deactivates a license key. Use POST with this path.</summary>
    public string DeactivateLicense() => "licenses/deactivate";

    // Meters

    /// <summary>Creates a meter. Use POST with this path.</summary>
    public string CreateMeter() => "meters";

    /// <summary>Lists meters. Use GET with this path.</summary>
    public string ListMeters() => "meters";

    /// <summary>Previews a meter definition. Use POST with this path.</summary>
    public string PreviewMeter() => "meters/preview";

    /// <summary>Reads, updates (PATCH) or archives (DELETE) one meter.</summary>
    public string Meter(string meterId) => $"meters/{Escape(meterId)}";

    /// <summary>Reads one meter.</summary>
    public string GetMeter(string meterId) => Meter(meterId);

    /// <summary>Updates one meter with PATCH.</summary>
    public string UpdateMeter(string meterId) => Meter(meterId);

    /// <summary>Previews an existing meter. Use POST with this path.</summary>
    public string PreviewMeter(string meterId) =>
        $"meters/{Escape(meterId)}/preview";

    /// <summary>Archives one meter. Use POST with this path.</summary>
    public string ArchiveMeter(string meterId) =>
        $"meters/{Escape(meterId)}/archive";

    /// <summary>Unarchives one meter. Use POST with this path.</summary>
    public string UnarchiveMeter(string meterId) =>
        $"meters/{Escape(meterId)}/unarchive";

    /// <summary>Reads the consumed units of one meter for a customer.</summary>
    public string GetMeterConsumedUnits(string meterId, string customerId) =>
        $"meters/{Escape(meterId)}/consumed?customer_id={Escape(customerId)}";

    // Usage events

    /// <summary>Previews usage events. Use POST with this path.</summary>
    public string PreviewUsageEvents() => "events/preview";

    /// <summary>Ingests usage events. Use POST with this path.</summary>
    public string IngestUsageEvents() => "events/ingest";

    /// <summary>Lists usage events.</summary>
    public string ListUsageEvents(int limit = DefaultPageSize) =>
        $"events?limit={limit}";

    // Customer credits

    /// <summary>Creates a credit account. Use POST with this path.</summary>
    public string CreateCreditAccount() => "customer-credits/accounts";

    /// <summary>Lists the credit accounts of one customer.</summary>
    public string ListCreditAccounts(string customerId) =>
        $"customer-credits/accounts?customer_id={Escape(customerId)}";

    /// <summary>Reads one credit account.</summary>
    public string GetCreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}";

    /// <summary>Reads the balance of one credit account.</summary>
    public string GetCreditBalance(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/balance";

    /// <summary>Credits one account. Use POST with this path.</summary>
    public string CreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/credit";

    /// <summary>Debits one account. Use POST with this path.</summary>
    public string DebitAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/debit";

    /// <summary>Lists the ledger entries of one credit account.</summary>
    public string ListCreditEntries(string accountId, int limit = DefaultPageSize) =>
        $"customer-credits/accounts/{Escape(accountId)}/entries?limit={limit}";

    /// <summary>Reverses a transaction on one account. Use POST with this path.</summary>
    public string ReverseCreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/reverse";

    /// <summary>Freezes one credit account. Use POST with this path.</summary>
    public string FreezeCreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/freeze";

    /// <summary>Unfreezes one credit account. Use POST with this path.</summary>
    public string UnfreezeCreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/unfreeze";

    /// <summary>Closes one credit account. Use POST with this path.</summary>
    public string CloseCreditAccount(string accountId) =>
        $"customer-credits/accounts/{Escape(accountId)}/close";

    /// <summary>Posts a credit transaction. Use POST with this path.</summary>
    public string PostCreditTransaction() => "customer-credits/transactions";

    /// <summary>Reads one credit transaction.</summary>
    public string GetCreditTransaction(string transactionId) =>
        $"customer-credits/transactions/{Escape(transactionId)}";

    /// <summary>Lists credit transactions by reference.</summary>
    public string ListCreditTransactions(string reference) =>
        $"customer-credits/transactions?reference={Escape(reference)}";

    /// <summary>Reverses one credit transaction. Use POST with this path.</summary>
    public string ReverseCreditTransaction(string transactionId) =>
        $"customer-credits/transactions/{Escape(transactionId)}/reverse";

    private static string Paged(string path, int pageNumber, int pageSize) =>
        $"{path}?page_number={pageNumber}&page_size={pageSize}";

    private static string Escape(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Uri.EscapeDataString(value);
    }
}
