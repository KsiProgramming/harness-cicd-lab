// Harness CI/CD lab: minimal API used as the CI target (Lab 3) and the canary workload (Lab 4).
// APP_VERSION is injected at image build time (Docker build-arg) so every response tells
// you which build, and which pod (hostname), served it.

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

// Liveness/readiness endpoint for Kubernetes probes (Lab 4).
app.MapHealthChecks("/healthz");

app.Run();

public record ServiceInfo(string Service, string Version, string Hostname);
public record Greeting(string Message);

public static class Greeter
{
    public static string Greet(string name) => $"Hello, {name.Trim()}!";
}

// Makes the implicit Program class visible to WebApplicationFactory<Program> in tests.
public partial class Program { }
