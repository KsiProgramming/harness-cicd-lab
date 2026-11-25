var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();

var version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "dev";

app.MapGet("/", () => Results.Ok(new ServiceInfo(
    Service: "harness-lab-api",
    Version: version,
    Hostname: Environment.MachineName)));

app.MapGet("/api/greeting/{name}", (string name) =>
    string.IsNullOrWhiteSpace(name)
        ? Results.BadRequest()
        : Results.Ok(new Greeting(Greeter.Greet(name))));

app.MapHealthChecks("/healthz");

app.Run();

public record ServiceInfo(string Service, string Version, string Hostname);
public record Greeting(string Message);

public static class Greeter
{
    public static string Greet(string name) => $"Hello, {name.Trim()}!";
}

public partial class Program { }
