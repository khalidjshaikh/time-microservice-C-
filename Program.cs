using TimeMicroservice;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content(TimeMicroserviceApp.BuildRootJson(), "application/json"));
app.MapGet("/healthz", () => Results.Content("{\"status\":\"ok\"}", "application/json"));
app.MapGet("/readyz", () => Results.Content("{\"status\":\"ok\"}", "application/json"));

var port = Environment.GetEnvironmentVariable("PORT");
if (string.IsNullOrWhiteSpace(port)) port = "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();