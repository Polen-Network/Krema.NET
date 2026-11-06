using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using KremaDotNet.Checkout;

namespace KremaDotNet.ApiTests;

/// <summary>
/// Thin wrapper around <see cref="HttpClient"/> used by the coverage runner.
/// </summary>
internal sealed class ApiClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly bool _verbose;

    public ApiClient(string baseUrl, string apiKey, bool verbose)
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri(baseUrl, UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(60),
        };

        _http.DefaultRequestHeaders.Add("x-api-key", apiKey);
        _http.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _verbose = verbose;
    }

    public async Task<ApiResponse> SendAsync(
        HttpMethod method,
        string path,
        object? body,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(method, path);
        string? requestBody = null;

        if (body is not null)
        {
            requestBody = JsonSerializer.Serialize(
                body,
                body.GetType(),
                CreemJson.Options);

            request.Content = new StringContent(
                requestBody,
                Encoding.UTF8,
                "application/json");
        }

        if (_verbose)
        {
            Console.WriteLine($"    > {method} {path}");
            if (requestBody is not null)
            {
                Console.WriteLine($"    > {Trim(requestBody)}");
            }
        }

        var stopwatch = Stopwatch.StartNew();
        using var response = await _http.SendAsync(
            request,
            HttpCompletionOption.ResponseContentRead,
            cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        stopwatch.Stop();

        if (_verbose)
        {
            Console.WriteLine($"    < {(int)response.StatusCode} {Trim(responseBody)}");
        }

        return new ApiResponse(
            method,
            path,
            (int)response.StatusCode,
            responseBody,
            stopwatch.Elapsed);
    }

    public void Dispose() => _http.Dispose();

    private static string Trim(string value)
    {
        const int limit = 500;

        return value.Length <= limit
            ? value
            : value[..limit] + "...";
    }
}

internal sealed record ApiResponse(
    HttpMethod Method,
    string Path,
    int StatusCode,
    string Body,
    TimeSpan Elapsed)
{
    public bool IsSuccess => StatusCode is >= 200 and < 300;

    public T? Deserialize<T>() =>
        JsonSerializer.Deserialize<T>(Body, CreemJson.Options);

    public string BodyPreview =>
        Body.Length <= 800 ? Body : Body[..800] + "...";
}
