using System.Net.Http;
using System.Text.Json;
using KremaDotNet.Accounts;
using KremaDotNet.Meters;
using KremaDotNet.Usage;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Runs the usage-based billing scenarios: meters, usage events and customer credits.
/// </summary>
internal static class UsageScenarios
{
    /// <summary>Runs all usage scenarios in order.</summary>
    public static async Task RunAsync(TestRun run)
    {
        await MetersAsync(run);
        await EventsAsync(run);
        await CreditsAsync(run);
    }

    /// <summary>Creates a meter, then lists, previews, reads, updates, archives and unarchives it.</summary>
    private static async Task MetersAsync(TestRun run)
    {
        var meter = await run.CallAsync<MeterApiResponseDto>(
            "Meters",
            "Create meter",
            HttpMethod.Post,
            "meters",
            new CreateMeterApiRequestDto
            {
                Name = $"Krema Test Meter {run.RunId}",
                EventName = run.EventName,
                Aggregation = MeterAggregation.Count,
                UnitLabel = "unit",
            });

        run.MeterId = meter?.Id;

        await run.CallAsync<MeterListApiResponseDto>(
            "Meters",
            "List meters",
            HttpMethod.Get,
            "meters");

        await run.CallAsync<MeterPreviewApiResponseDto>(
            "Meters",
            "Preview meter definition",
            HttpMethod.Post,
            "meters/preview",
            new PreviewMeterApiRequestDto
            {
                EventName = run.EventName,
                Aggregation = MeterAggregation.Count,
                UnitLabel = "unit",
            });

        if (run.MeterId is null)
        {
            run.Skip("Meters", "Get meter", "Meter creation failed.");
            run.Skip("Meters", "Update meter", "Meter creation failed.");
            run.Skip("Meters", "Preview existing meter", "Meter creation failed.");
            run.Skip("Meters", "Archive meter", "Meter creation failed.");
            run.Skip("Meters", "Unarchive meter", "Meter creation failed.");
            return;
        }

        var meterId = Uri.EscapeDataString(run.MeterId);

        await run.CallAsync<MeterApiResponseDto>(
            "Meters",
            "Get meter",
            HttpMethod.Get,
            $"meters/{meterId}");

        await run.CallAsync<MeterApiResponseDto>(
            "Meters",
            "Update meter",
            HttpMethod.Patch,
            $"meters/{meterId}",
            new UpdateMeterApiRequestDto
            {
                Name = $"Krema Test Meter {run.RunId} Updated",
            });

        await run.CallAsync<MeterPreviewApiResponseDto>(
            "Meters",
            "Preview existing meter",
            HttpMethod.Post,
            $"meters/{meterId}/preview");

        await run.CallAsync<MeterApiResponseDto>(
            "Meters",
            "Archive meter",
            HttpMethod.Post,
            $"meters/{meterId}/archive");

        await run.CallAsync<MeterApiResponseDto>(
            "Meters",
            "Unarchive meter",
            HttpMethod.Post,
            $"meters/{meterId}/unarchive");
    }

    /// <summary>Previews and ingests a usage event, lists events and reads the meter's consumed units.</summary>
    private static async Task EventsAsync(TestRun run)
    {
        var events = new IngestUsageEventsApiRequestDto
        {
            Events = new List<IngestUsageEventApiRequestDto>
            {
                new()
                {
                    Name = run.EventName,
                    CustomerId = run.CustomerId,
                    EventId = $"evt-{run.RunId}",
                    Timestamp = DateTimeOffset.UtcNow.ToString("o"),
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["source"] = JsonSerializer.SerializeToElement("krema-dotnet-api-tests"),
                    },
                },
            },
        };

        if (run.CustomerId is null)
        {
            run.Skip("Usage", "Preview events", "No customer available.");
            run.Skip("Usage", "Ingest events", "No customer available.");
        }
        else
        {
            await run.CallAsync<PreviewUsageEventsApiResponseDto>(
                "Usage",
                "Preview events",
                HttpMethod.Post,
                "events/preview",
                events);

            await run.CallAsync<IngestUsageEventsApiResponseDto>(
                "Usage",
                "Ingest events",
                HttpMethod.Post,
                "events/ingest",
                events);
        }

        await run.CallAsync<UsageEventListApiResponseDto>(
            "Usage",
            "List events",
            HttpMethod.Get,
            "events?limit=10");

        if (run.MeterId is null || run.CustomerId is null)
        {
            run.Skip("Usage", "Get consumed units", "No meter or customer available.");
            return;
        }

        // Give the ingestion pipeline a moment so the consumption report is available.
        await Task.Delay(TimeSpan.FromSeconds(1));

        await run.CallAsync<MeterConsumedUnitsApiResponseDto>(
            "Usage",
            "Get consumed units",
            HttpMethod.Get,
            $"meters/{Uri.EscapeDataString(run.MeterId)}/consumed" +
            $"?customer_id={Uri.EscapeDataString(run.CustomerId)}");
    }

    /// <summary>Creates three credit accounts, then exercises balances, transactions, reversals, freeze, unfreeze and close.</summary>
    private static async Task CreditsAsync(TestRun run)
    {
        if (run.CustomerId is null)
        {
            run.Skip("Credits", "Create account A", "No customer available.");
            run.Skip("Credits", "Create account B", "No customer available.");
            run.Skip("Credits", "Create account C", "No customer available.");
            run.Skip("Credits", "List accounts", "No customer available.");
            run.Skip("Credits", "Get account", "No customer available.");
            run.Skip("Credits", "Get balance", "No customer available.");
            run.Skip("Credits", "Credit account", "No customer available.");
            run.Skip("Credits", "Debit account", "No customer available.");
            run.Skip("Credits", "Post transaction", "No customer available.");
            run.Skip("Credits", "Get transaction", "No customer available.");
            run.Skip("Credits", "List transactions by reference", "No customer available.");
            run.Skip("Credits", "List entries", "No customer available.");
            run.Skip("Credits", "Reverse transaction by id", "No customer available.");
            run.Skip("Credits", "Reverse transaction by account", "No customer available.");
            run.Skip("Credits", "Freeze account", "No customer available.");
            run.Skip("Credits", "Unfreeze account", "No customer available.");
            run.Skip("Credits", "Close account", "No customer available.");
            return;
        }

        var accountA = await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Create account A",
            HttpMethod.Post,
            "customer-credits/accounts",
            new CreateAccountDto
            {
                Name = $"Krema Credits A {run.RunId}",
                CustomerId = run.CustomerId,
                UnitLabel = "credit",
                InitialBalance = "100",
            });

        var accountB = await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Create account B",
            HttpMethod.Post,
            "customer-credits/accounts",
            new CreateAccountDto
            {
                Name = $"Krema Credits B {run.RunId}",
                CustomerId = run.CustomerId,
                UnitLabel = "credit",
                InitialBalance = "0",
            });

        var accountC = await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Create account C",
            HttpMethod.Post,
            "customer-credits/accounts",
            new CreateAccountDto
            {
                Name = $"Krema Credits C {run.RunId}",
                CustomerId = run.CustomerId,
                UnitLabel = "credit",
                InitialBalance = "0",
            });

        run.CreditAccountAId = accountA?.Id;
        run.CreditAccountBId = accountB?.Id;
        run.CreditAccountCId = accountC?.Id;

        await run.CallAsync<AccountListResponseDto>(
            "Credits",
            "List accounts",
            HttpMethod.Get,
            $"customer-credits/accounts?customer_id={Uri.EscapeDataString(run.CustomerId)}");

        if (run.CreditAccountAId is null || run.CreditAccountBId is null)
        {
            run.Skip("Credits", "Get account", "Credit account creation failed.");
            run.Skip("Credits", "Get balance", "Credit account creation failed.");
            run.Skip("Credits", "Credit account", "Credit account creation failed.");
            run.Skip("Credits", "Debit account", "Credit account creation failed.");
            run.Skip("Credits", "Post transaction", "Credit account creation failed.");
            run.Skip("Credits", "Get transaction", "Credit account creation failed.");
            run.Skip("Credits", "List transactions by reference", "Credit account creation failed.");
            run.Skip("Credits", "List entries", "Credit account creation failed.");
            run.Skip("Credits", "Reverse transaction by id", "Credit account creation failed.");
            run.Skip("Credits", "Reverse transaction by account", "Credit account creation failed.");
        }
        else
        {
            await CreditsWithAccountsAsync(run);
        }

        if (run.CreditAccountCId is null)
        {
            run.Skip("Credits", "Freeze account", "Lifecycle account creation failed.");
            run.Skip("Credits", "Unfreeze account", "Lifecycle account creation failed.");
            run.Skip("Credits", "Close account", "Lifecycle account creation failed.");
            return;
        }

        var accountCPath = Uri.EscapeDataString(run.CreditAccountCId);

        await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Freeze account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountCPath}/freeze");

        await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Unfreeze account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountCPath}/unfreeze");

        await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Close account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountCPath}/close");
    }

    /// <summary>Reads the account, credits, debits, posts and reverses transactions and lists entries and references.</summary>
    private static async Task CreditsWithAccountsAsync(TestRun run)
    {
        var accountA = Uri.EscapeDataString(run.CreditAccountAId!);
        var accountB = Uri.EscapeDataString(run.CreditAccountBId!);

        await run.CallAsync<AccountResponseDto>(
            "Credits",
            "Get account",
            HttpMethod.Get,
            $"customer-credits/accounts/{accountA}");

        await run.CallAsync<BalanceResponseDto>(
            "Credits",
            "Get balance",
            HttpMethod.Get,
            $"customer-credits/accounts/{accountA}/balance");

        var credit = await run.CallAsync<TransactionResponseDto>(
            "Credits",
            "Credit account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountA}/credit",
            new CreditDebitRequestDto
            {
                Amount = "10",
                Reference = $"credit-{run.RunId}",
                IdempotencyKey = $"credit-key-{run.RunId}",
            });

        await run.CallAsync<TransactionResponseDto>(
            "Credits",
            "Debit account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountA}/debit",
            new CreditDebitRequestDto
            {
                Amount = "10",
                Reference = $"debit-{run.RunId}",
                IdempotencyKey = $"debit-key-{run.RunId}",
            });

        var posted = await run.CallAsync<TransactionResponseDto>(
            "Credits",
            "Post transaction",
            HttpMethod.Post,
            "customer-credits/transactions",
            new PostTransactionDto
            {
                Reference = $"transfer-{run.RunId}",
                IdempotencyKey = $"transfer-key-{run.RunId}",
                Entries = new List<EntryInputDto>
                {
                    new()
                    {
                        AccountId = run.CreditAccountAId!,
                        Side = EntrySide.Debit,
                        Amount = "10",
                    },
                    new()
                    {
                        AccountId = run.CreditAccountBId!,
                        Side = EntrySide.Credit,
                        Amount = "10",
                    },
                },
            });

        if (posted is null)
        {
            run.Skip("Credits", "Get transaction", "Post transaction failed.");
            run.Skip("Credits", "List transactions by reference", "Post transaction failed.");
            run.Skip("Credits", "List entries", "Post transaction failed.");
            run.Skip("Credits", "Reverse transaction by id", "Post transaction failed.");
        }
        else
        {
            await run.CallAsync<TransactionResponseDto>(
                "Credits",
                "Get transaction",
                HttpMethod.Get,
                $"customer-credits/transactions/{Uri.EscapeDataString(posted.Id)}");

            await run.CallAsync<TransactionListResponseDto>(
                "Credits",
                "List transactions by reference",
                HttpMethod.Get,
                $"customer-credits/transactions?reference={Uri.EscapeDataString(posted.Reference)}");

            await run.CallAsync<EntryListResponseDto>(
                "Credits",
                "List entries",
                HttpMethod.Get,
                $"customer-credits/accounts/{accountA}/entries?limit=10");

            await run.CallAsync<TransactionResponseDto>(
                "Credits",
                "Reverse transaction by id",
                HttpMethod.Post,
                $"customer-credits/transactions/{Uri.EscapeDataString(posted.Id)}/reverse");
        }

        if (credit is null)
        {
            run.Skip("Credits", "Reverse transaction by account", "Credit transaction failed.");
            return;
        }

        await run.CallAsync<TransactionResponseDto>(
            "Credits",
            "Reverse transaction by account",
            HttpMethod.Post,
            $"customer-credits/accounts/{accountA}/reverse",
            new ReverseTransactionRequestDto
            {
                TransactionId = credit.Id,
            });
    }
}
