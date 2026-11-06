using System.Text.Json.Serialization;

namespace Krema.NET.Accounts;

[JsonConverter(typeof(JsonStringEnumConverter<AccountStatus>))]
public enum AccountStatus
{
    [JsonStringEnumMemberName("active")] Active,
    [JsonStringEnumMemberName("frozen")] Frozen,
    [JsonStringEnumMemberName("closed")] Closed,
}

[JsonConverter(typeof(JsonStringEnumConverter<EntrySide>))]
public enum EntrySide
{
    [JsonStringEnumMemberName("debit")] Debit,
    [JsonStringEnumMemberName("credit")] Credit,
}

/// <summary>
/// A credit account belonging to a customer.
/// </summary>
public class AccountResponseDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("store_id")]
    public required string StoreId { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("unit_label")]
    public required string UnitLabel { get; set; }

    [JsonPropertyName("status")]
    public required AccountStatus Status { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public required string UpdatedAt { get; set; }
}

/// <summary>
/// Paginated account list.
/// </summary>
public class AccountListResponseDto
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("data")]
    public required List<AccountResponseDto> Data { get; set; }

    [JsonPropertyName("has_more")]
    public required bool HasMore { get; set; }
}

/// <summary>
/// Request body for creating a credit account.
/// </summary>
public class CreateAccountDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("customer_id")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("unit_label")]
    public string? UnitLabel { get; set; }

    [JsonPropertyName("initial_balance")]
    public string? InitialBalance { get; set; }
}

/// <summary>
/// Balance of a credit account.
/// </summary>
public class BalanceResponseDto
{
    [JsonPropertyName("balance")]
    public required string Balance { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("as_of")]
    public string? AsOf { get; set; }
}

/// <summary>
/// A ledger entry sent when posting a transaction.
/// </summary>
public class EntryInputDto
{
    [JsonPropertyName("account_id")]
    public required string AccountId { get; set; }

    [JsonPropertyName("side")]
    public required EntrySide Side { get; set; }

    [JsonPropertyName("amount")]
    public required string Amount { get; set; }
}

/// <summary>
/// A ledger entry returned by the API.
/// </summary>
public class EntryResponseDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; set; }

    [JsonPropertyName("account_id")]
    public required string AccountId { get; set; }

    [JsonPropertyName("side")]
    public required EntrySide Side { get; set; }

    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }
}

/// <summary>
/// Paginated ledger entry list.
/// </summary>
public class EntryListResponseDto
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("data")]
    public required List<EntryResponseDto> Data { get; set; }

    [JsonPropertyName("has_more")]
    public required bool HasMore { get; set; }
}

/// <summary>
/// Request body for posting a ledger transaction.
/// </summary>
public class PostTransactionDto
{
    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("idempotency_key")]
    public required string IdempotencyKey { get; set; }

    [JsonPropertyName("entries")]
    public required List<EntryInputDto> Entries { get; set; }
}

/// <summary>
/// A posted ledger transaction.
/// </summary>
public class TransactionResponseDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("store_id")]
    public required string StoreId { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("idempotency_key")]
    public required string IdempotencyKey { get; set; }

    [JsonPropertyName("reversal_of")]
    public string? ReversalOf { get; set; }

    [JsonPropertyName("entries")]
    public required List<EntryResponseDto> Entries { get; set; }

    [JsonPropertyName("created_at")]
    public required string CreatedAt { get; set; }
}

/// <summary>
/// Paginated ledger transaction list.
/// </summary>
public class TransactionListResponseDto
{
    [JsonPropertyName("object")]
    public required string Object { get; set; }

    [JsonPropertyName("data")]
    public required List<TransactionResponseDto> Data { get; set; }

    [JsonPropertyName("has_more")]
    public required bool HasMore { get; set; }
}

/// <summary>
/// Request body for reversing a ledger transaction.
/// </summary>
public class ReverseTransactionRequestDto
{
    [JsonPropertyName("transaction_id")]
    public required string TransactionId { get; set; }
}

/// <summary>
/// Request body for crediting or debiting a credit account.
/// </summary>
public class CreditDebitRequestDto
{
    [JsonPropertyName("amount")]
    public required string Amount { get; set; }

    [JsonPropertyName("reference")]
    public required string Reference { get; set; }

    [JsonPropertyName("idempotency_key")]
    public required string IdempotencyKey { get; set; }
}
