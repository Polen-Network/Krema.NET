using System.Text.Json;
using KremaDotNet.Checkout;
using KremaDotNet.Customers;
using KremaDotNet.Products;
using KremaDotNet.Subscriptions;
using KremaDotNet.Webhooks;

namespace KremaDotNet.Tests;

public class SerializationTests
{
    [Fact]
    public void Product_DeserializesSnakeCaseAndEnums()
    {
        const string json = """
        {
          "id": "prod_1",
          "mode": "test",
          "object": "product",
          "name": "Test",
          "description": "A test product",
          "price": 100,
          "currency": "USD",
          "billing_type": "onetime",
          "billing_period": "once",
          "status": "active",
          "tax_mode": "inclusive",
          "tax_category": "saas",
          "created_at": "2026-01-01T00:00:00.000Z",
          "updated_at": "2026-01-01T00:00:00.000Z"
        }
        """;

        var product = JsonSerializer.Deserialize<ProductEntity>(json, CreemJson.Options);

        Assert.NotNull(product);
        Assert.Equal("prod_1", product!.Id);
        Assert.Equal(100m, product.Price);
        Assert.Equal(ProductBillingType.Onetime, product.BillingType);
        Assert.Equal(ProductBillingPeriod.Once, product.BillingPeriod);
        Assert.Equal(ProductStatus.Active, product.Status);
        Assert.Equal(TaxMode.Inclusive, product.TaxMode);
        Assert.Equal(TaxCategory.Saas, product.TaxCategory);
    }

    [Fact]
    public void CreateProductRequest_SerializesSnakeCaseAndOmitsNulls()
    {
        var request = new CreateProductRequestEntity
        {
            Name = "Pro",
            Description = "Pro plan",
            Price = 1000,
            Currency = ProductCurrency.Usd,
            BillingType = ProductRequestBillingType.Recurring,
            BillingPeriod = ProductRequestBillingPeriod.EveryMonth,
        };

        var json = JsonSerializer.Serialize(request, CreemJson.Options);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal("USD", root.GetProperty("currency").GetString());
        Assert.Equal("recurring", root.GetProperty("billing_type").GetString());
        Assert.Equal("every-month", root.GetProperty("billing_period").GetString());
        Assert.False(root.TryGetProperty("image_url", out _));
        Assert.False(root.TryGetProperty("trial_price", out _));
    }

    [Fact]
    public void Subscription_ReadsUnionMembersAsIdOrObject()
    {
        const string json = """
        {
          "id": "sub_1",
          "mode": "test",
          "object": "subscription",
          "product": "prod_1",
          "customer": {
            "id": "cus_1",
            "mode": "test",
            "object": "customer",
            "email": "customer@example.com",
            "country": null,
            "created_at": "2026-01-01T00:00:00.000Z",
            "updated_at": "2026-01-01T00:00:00.000Z"
          },
          "collection_method": "charge_automatically",
          "status": "active",
          "created_at": "2026-01-01T00:00:00.000Z",
          "updated_at": "2026-01-01T00:00:00.000Z"
        }
        """;

        var subscription = JsonSerializer.Deserialize<SubscriptionEntity>(json, CreemJson.Options);

        Assert.NotNull(subscription);
        Assert.Equal("prod_1", CreemJson.GetId(subscription!.Product));

        var customer = CreemJson.GetObject<CustomerEntity>(subscription.Customer);
        Assert.NotNull(customer);
        Assert.Equal("cus_1", customer!.Id);
        Assert.Equal("customer@example.com", customer.Email);
        Assert.Equal(SubscriptionStatus.Active, subscription.Status);
    }

    [Fact]
    public void WebhookEvent_SelectsConcreteTypeFromEventType()
    {
        const string json = """
        {
          "id": "evt_1",
          "eventType": "checkout.completed",
          "created_at": 1700000000,
          "object": {
            "id": "ch_1",
            "mode": "test",
            "object": "checkout",
            "status": "completed",
            "product": "prod_1"
          }
        }
        """;

        var webhookEvent = JsonSerializer.Deserialize<WebhookEventEntity>(json, CreemJson.Options);

        var checkoutEvent = Assert.IsType<WebhookCheckoutCompletedEventEntity>(webhookEvent);
        Assert.Equal("evt_1", checkoutEvent.Id);
        Assert.Equal(1700000000, checkoutEvent.CreatedAt);
        Assert.Equal("ch_1", checkoutEvent.Object.Id);
    }
}
