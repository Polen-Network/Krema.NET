using System.Diagnostics;
using Krema.NET.ApiTests;

if (args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine("Krema.NET API coverage runner");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  dotnet run --project test/Krema.NET.ApiTests [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --verbose      Print request and response bodies.");
    Console.WriteLine("  --help         Show this help.");
    Console.WriteLine();
    Console.WriteLine("The runner refuses to start unless CREEM_MODE is 'test' and the API key starts with 'creem_test_'.");
    return 0;
}

var verbose = args.Contains("--verbose");

string mode;
string apiKey;
string baseUrl;

try
{
    mode = CreemConfiguration.GetMode();
    apiKey = CreemConfiguration.GetApiKey();
    baseUrl = CreemConfiguration.GetBaseUrl();
}
catch (InvalidOperationException ex)
{
    Console.Error.WriteLine(ex.Message);
    Console.Error.WriteLine("Create a .env file in the repository root with CREEM_API_KEY and CREEM_MODE=test.");
    return 2;
}

if (!string.Equals(mode, "test", StringComparison.OrdinalIgnoreCase))
{
    Console.Error.WriteLine($"Refusing to run: CREEM_MODE is '{mode}', but this runner only works in test mode.");
    return 2;
}

if (!apiKey.StartsWith("creem_test_", StringComparison.Ordinal))
{
    Console.Error.WriteLine("Refusing to run: the API key does not look like a Creem test key (creem_test_...).");
    return 2;
}

Console.WriteLine("Krema.NET API coverage runner");
Console.WriteLine($"  Mode:        {mode}");
Console.WriteLine($"  Base URL:    {baseUrl}");
Console.WriteLine($"  API key:     {Mask(apiKey)}");
Console.WriteLine($"  Verbose:     {verbose}");

using var client = new ApiClient(baseUrl, apiKey, verbose);
var run = new TestRun(client);
var stopwatch = Stopwatch.StartNew();

Console.WriteLine($"  Run ID:      {run.RunId}");
Console.WriteLine();
Console.WriteLine("Data-dependent endpoints (transactions, subscriptions, licenses) are skipped when the");
Console.WriteLine("test account has no matching resource.");

await CommerceScenarios.RunAsync(run);
await UsageScenarios.RunAsync(run);
await PlatformScenarios.RunAsync(run);
await BillingScenarios.RunAsync(run);
await CleanupScenarios.RunAsync(run);

stopwatch.Stop();

return run.PrintSummary(stopwatch.Elapsed);

static string Mask(string key) =>
    key.Length <= 16
        ? key[..Math.Min(8, key.Length)] + "..."
        : key[..12] + "..." + key[^4..];
