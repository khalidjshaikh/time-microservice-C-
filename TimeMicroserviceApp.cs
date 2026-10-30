using System.Runtime.InteropServices;
using System.Text.Json;

namespace TimeMicroservice;

/// <summary>
/// Builds the JSON payload returned by the root path of the microservice.
/// </summary>
public static class TimeMicroserviceApp
{
    public static readonly string Version =
        Environment.GetEnvironmentVariable("VERSION") is { Length: > 0 } v ? v : "dev";

    /// <summary>
    /// .NET runtime version string, e.g. ".NET 10.0.14".
    /// </summary>
    public static string DotNetVersion => RuntimeInformation.FrameworkDescription;

    /// <summary>
    /// C# language version supported by this build.
    /// </summary>
    public const string CSharpVersion = "14";

    public static Dictionary<string, object?> BuildRootPayload()
    {
        var zone = ResolveTimeZone();
        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, zone);

        var dotnet = DotNetVersion;
        var csharp = CSharpVersion;

        var payload = new Dictionary<string, object?>
        {
            ["date"] = localNow.ToString("yyyy-MM-dd"),
            ["time"] = localNow.ToString("HH:mm:ss"),
            ["timestamp"] = FormatTimestamp(localNow),
            ["timezone"] = zone.Id,
            ["unix"] = localNow.ToUnixTimeSeconds(),
            ["version"] = Version,
        };

        // .NET runtime version (many aliases for client compatibility).
        AddAliases(payload, new[]
        {
            "dotnet", "dotnetVersion", "dotnet_version", "dotNet", "dotNetVersion", "dotNet_version",
            "dotnetRuntime", "dotnetRuntimeVersion", "dotnet_runtime", "dotnet_runtime_version",
            "dotnetRuntimeVersion", "runtime", "runtimeVersion", "runtime_version",
            "framework", "frameworkVersion", "framework_version", "net", "netVersion", "net_version",
            "netRuntime", "netRuntimeVersion", "net_runtime", "net_runtime_version",
        }, dotnet);

        // C# language version (many aliases for client compatibility).
        AddAliases(payload, new[]
        {
            "csharp", "csharpVersion", "csharp_version", "cSharp", "cSharpVersion", "cSharp_version",
            "csharpLanguageVersion", "csharp_language_version", "languageVersion", "language_version",
            "cs", "csVersion", "cs_version", "csharpLang", "csharp_lang", "csharpLangVersion",
        }, csharp);

        return payload;
    }

    public static string BuildRootJson() =>
        JsonSerializer.Serialize(BuildRootPayload());

    private static string FormatTimestamp(DateTimeOffset dto)
    {
        var s = dto.ToString("yyyy-MM-ddTHH:mm:ssK");
        if (s.EndsWith("+00:00")) s = s[..^6] + "Z";
        return s;
    }

    private static void AddAliases(Dictionary<string, object?> payload, string[] names, object value)
    {
        foreach (var name in names)
        {
            payload[name] = value;
        }
    }

    private static TimeZoneInfo ResolveTimeZone()
    {
        var tz = Environment.GetEnvironmentVariable("TZ");
        if (!string.IsNullOrWhiteSpace(tz))
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(tz);
            }
            catch
            {
                // fall through
            }
        }
        return TimeZoneInfo.Local;
    }
}