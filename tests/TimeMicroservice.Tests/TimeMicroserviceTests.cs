using TimeMicroservice;
using Xunit;

namespace TimeMicroservice.Tests;

public class TimeMicroserviceTests
{
    [Fact]
    public void BuildRootPayload_HasRequiredFields()
    {
        var payload = TimeMicroserviceApp.BuildRootPayload();
        Assert.True(payload.ContainsKey("date"));
        Assert.True(payload.ContainsKey("time"));
        Assert.True(payload.ContainsKey("timestamp"));
        Assert.True(payload.ContainsKey("timezone"));
        Assert.True(payload.ContainsKey("unix"));
        Assert.True(payload.ContainsKey("version"));
        Assert.NotEmpty(payload["date"]?.ToString());
        Assert.NotEmpty(payload["time"]?.ToString());
        Assert.True((long)(payload["unix"]!) > 0);
    }

    [Fact]
    public void BuildRootJson_ContainsRuntimeVersion()
    {
        var json = TimeMicroserviceApp.BuildRootJson();
        Assert.Contains("\"dotnet", json);
        Assert.Contains("\"csharp", json);
    }

    [Fact]
    public void HealthPayload_Ok()
    {
        // sanity: constants non-empty
        Assert.NotEmpty(TimeMicroserviceApp.Version);
        Assert.NotEmpty(TimeMicroserviceApp.DotNetVersion);
        Assert.Equal("14", TimeMicroserviceApp.CSharpVersion);
    }
}