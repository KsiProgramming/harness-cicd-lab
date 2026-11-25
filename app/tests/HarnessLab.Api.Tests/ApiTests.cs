using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HarnessLab.Api.Tests;

// Unit tests: pure logic, no HTTP.
public class GreeterTests
{
    [Theory]
    [InlineData("Harness", "Hello, Harness!")]
    [InlineData("  padded  ", "Hello, padded!")]
    public void Greet_returns_trimmed_greeting(string input, string expected) =>
        Assert.Equal(expected, Greeter.Greet(input));
}

// Integration tests: the real ASP.NET Core pipeline hosted in memory (no Docker, no network).
public class ApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Root_returns_service_info()
    {
        var info = await _client.GetFromJsonAsync<ServiceInfo>("/");

        Assert.NotNull(info);
        Assert.Equal("harness-lab-api", info.Service);
        Assert.False(string.IsNullOrWhiteSpace(info.Version));
        Assert.False(string.IsNullOrWhiteSpace(info.Hostname));
    }

    [Fact]
    public async Task Greeting_endpoint_greets_by_name()
    {
        var greeting = await _client.GetFromJsonAsync<Greeting>("/api/greeting/Harness");

        Assert.Equal("Hello, Harness!", greeting?.Message);
    }

    [Fact]
    public async Task Health_endpoint_is_healthy()
    {
        var response = await _client.GetAsync("/healthz");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}
