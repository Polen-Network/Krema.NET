# KremaDotNET Documents

## What is where

| Path | What it is |
| --- | --- |
| `src/KremaDotNet/KremaDotNet.csproj` | The Creem API client library. |
| `src/KremaDotNet/<Domain>/Types.cs` | Request/response types per domain (Products, Customers, Subscriptions, ...). |
| `src/KremaDotNet/Config/CreemConfigration.cs` | Loads `.env`, API key, mode and base URL. |
| `test/KremaDotNet.Tests/` | xUnit tests (serialization + optional test-mode API call). |
| `test/KremaDotNet.ApiTests/` | Console runner that calls the Creem endpoints and prints a report. |

## Setup

Create `.env` in the repository root:

```ini
CREEM_API_KEY=creem_test_...
CREEM_MODE=test
```

The runner refuses to start unless `CREEM_MODE=test` and the key starts with `creem_test_`.

## Commands

```powershell
dotnet build KremaDotNet.slnx
dotnet test KremaDotNet.slnx

# API coverage
dotnet run --project test/KremaDotNet.ApiTests

# Print request and response bodies
dotnet run --project test/KremaDotNet.ApiTests -- --verbose
```

The runner does not refund transactions or cancel/upgrade subscriptions.

## Report

- `PASS` — endpoint worked and the response mapped to the model.
- `FAIL` — HTTP error or model mismatch; details are printed.
- `SKIP` — missing test data.
- Exit code `1` when any step fails.
