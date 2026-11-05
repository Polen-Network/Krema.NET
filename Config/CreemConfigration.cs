using System;
using System.Collections.Generic;
using System.IO;

public static class CreemConfiguration
{
    private static readonly Dictionary<string, string> DotEnv = LoadDotEnv();

    public static string GetApiKey()
    {
        var apiKey = Environment.GetEnvironmentVariable("CREEM_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
            DotEnv.TryGetValue("CREEM_API_KEY", out apiKey);

        if (!string.IsNullOrWhiteSpace(apiKey))
            return apiKey;

        throw new InvalidOperationException(
            "API key not found. Set CREEM_API_KEY in your environment or in a .env file."
        );
    }

    public static string GetMode()
    {
        var mode = Environment.GetEnvironmentVariable("CREEM_MODE");

        if (string.IsNullOrWhiteSpace(mode))
            DotEnv.TryGetValue("CREEM_MODE", out mode);

        return string.IsNullOrWhiteSpace(mode)
            ? "test"
            : mode.ToLowerInvariant();
    }

    public static string GetBaseUrl()
    {
        return GetMode() == "prod"
            ? "https://api.creem.io/v1/"
            : "https://test-api.creem.io/v1/";
    }

    /// <summary>
    /// Looks for a <c>.env</c> file starting in the current directory and walking
    /// up its parents. Environment variables always take precedence over it.
    /// </summary>
    private static Dictionary<string, string> LoadDotEnv()
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            var path = Path.Combine(directory.FullName, ".env");

            if (File.Exists(path))
            {
                LoadFile(path, values);
                break;
            }

            directory = directory.Parent;
        }

        return values;
    }

    private static void LoadFile(string path, Dictionary<string, string> values)
    {
        foreach (var rawLine in File.ReadAllLines(path))
        {
            var line = rawLine.Trim();

            if (line.Length == 0 || line.StartsWith('#'))
                continue;

            var separator = line.IndexOf('=');
            if (separator <= 0)
                continue;

            var key = line[..separator].Trim();
            var value = line[(separator + 1)..].Trim().Trim('"', '\'');
            values[key] = value;
        }
    }
}
