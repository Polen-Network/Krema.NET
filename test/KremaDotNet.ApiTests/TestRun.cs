using System.Net.Http;
using System.Text.Json;

namespace KremaDotNet.ApiTests;

internal enum StepStatus
{
    Pass,
    Fail,
    Skip,
}

internal sealed record StepResult(
    string Area,
    string Name,
    string Method,
    string Path,
    StepStatus Status,
    int? StatusCode,
    string Detail,
    TimeSpan Elapsed);

/// <summary>
/// Runs API calls in order, records the result of every step and keeps the
/// identifiers needed by later scenarios.
/// </summary>
internal sealed class TestRun
{
    private readonly ApiClient _client;
    private readonly List<StepResult> _results = new();
    private string? _lastArea;

    public TestRun(ApiClient client)
    {
        _client = client;
    }

    /// <summary>Alphanumeric suffix that makes every created resource unique.</summary>
    public string RunId { get; } =
        DateTime.UtcNow.ToString("yyMMddHHmmss") + Random.Shared.Next(1000, 10000);

    public string EventName => $"krema_test_event_{RunId}";

    public string? ProductId { get; set; }
    public string? RecurringProductId { get; set; }
    public string? ExistingProductId { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CheckoutId { get; set; }
    public string? DiscountId { get; set; }
    public string? DiscountCode { get; set; }
    public string? MeterId { get; set; }
    public string? AffiliateId { get; set; }
    public string? SplitId { get; set; }
    public string? LicenseId { get; set; }
    public string? LicenseKey { get; set; }
    public string? CreditAccountAId { get; set; }
    public string? CreditAccountBId { get; set; }
    public string? CreditAccountCId { get; set; }
    public string? SubscriptionId { get; set; }

    public async Task<T?> CallAsync<T>(
        string area,
        string name,
        HttpMethod method,
        string path,
        object? body = null)
    {
        try
        {
            var response = await _client.SendAsync(method, path, body);

            if (!response.IsSuccess)
            {
                Record(
                    area,
                    name,
                    method.Method,
                    path,
                    StepStatus.Fail,
                    response.StatusCode,
                    response.BodyPreview,
                    response.Elapsed);

                return default;
            }

            if (string.IsNullOrWhiteSpace(response.Body))
            {
                Record(area, name, method.Method, path, StepStatus.Pass, response.StatusCode, "empty body", response.Elapsed);
                return default;
            }

            T? value;

            try
            {
                value = response.Deserialize<T>();
            }
            catch (JsonException ex)
            {
                Record(
                    area,
                    name,
                    method.Method,
                    path,
                    StepStatus.Fail,
                    response.StatusCode,
                    $"Response could not be deserialized as {typeof(T).Name}: {ex.Message}",
                    response.Elapsed);

                return default;
            }

            Record(area, name, method.Method, path, StepStatus.Pass, response.StatusCode, "ok", response.Elapsed);

            return value;
        }
        catch (Exception ex)
        {
            Record(area, name, method.Method, path, StepStatus.Fail, null, ex.Message, TimeSpan.Zero);
            return default;
        }
    }

    public void Skip(string area, string name, string reason)
    {
        Record(area, name, "-", "-", StepStatus.Skip, null, reason, TimeSpan.Zero);
    }

    public int PrintSummary(TimeSpan total)
    {
        var passed = _results.Count(r => r.Status == StepStatus.Pass);
        var failed = _results.Count(r => r.Status == StepStatus.Fail);
        var skipped = _results.Count(r => r.Status == StepStatus.Skip);

        Console.WriteLine();
        Console.WriteLine(new string('-', 100));
        Console.WriteLine(
            $"SUMMARY: {passed} passed, {failed} failed, {skipped} skipped, {_results.Count} total " +
            $"in {total.TotalSeconds:0.0}s");

        if (failed > 0)
        {
            Console.WriteLine();
            Console.WriteLine("FAILED STEPS");

            foreach (var step in _results.Where(r => r.Status == StepStatus.Fail))
            {
                Console.WriteLine($"  {step.Area} / {step.Name} ({step.Method} {step.Path})");
                Console.WriteLine($"    {step.Detail}");
            }
        }

        if (skipped > 0)
        {
            Console.WriteLine();
            Console.WriteLine("SKIPPED STEPS");

            foreach (var step in _results.Where(r => r.Status == StepStatus.Skip))
            {
                Console.WriteLine($"  {step.Area} / {step.Name}: {step.Detail}");
            }
        }

        return failed > 0 ? 1 : 0;
    }

    private void Record(
        string area,
        string name,
        string method,
        string path,
        StepStatus status,
        int? statusCode,
        string detail,
        TimeSpan elapsed)
    {
        if (!string.Equals(_lastArea, area, StringComparison.Ordinal))
        {
            Console.WriteLine();
            Console.WriteLine($"[{area}]");
            _lastArea = area;
        }

        var label = status switch
        {
            StepStatus.Pass => "PASS",
            StepStatus.Fail => "FAIL",
            _ => "SKIP",
        };

        var methodText = method;
        var codeText = statusCode?.ToString() ?? "-";

        Console.WriteLine(
            $"  {label,-4} {methodText,-6} {name,-46} {codeText,4}  {elapsed.TotalMilliseconds,6:0}ms");

        _results.Add(new StepResult(area, name, method, path, status, statusCode, detail, elapsed));
    }
}
