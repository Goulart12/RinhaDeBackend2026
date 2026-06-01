using System.Text.Json;
using RinhaDeBackend2026.Models.Dataset;
using RinhaDeBackend2026.Services;
using RinhaDeBackend2026.Services.Dataset;
using RinhaDeBackend2026.Services.Fraud;
using RinhaDeBackend2026.Services.Search;
using RinhaDeBackend2026.Services.Vectorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.Configure<DatasetOptions>(
    builder.Configuration.GetSection("Dataset"));

builder.Services.AddSingleton<
    IVectorizationService,
    VectorizationService>();

builder.Services.AddSingleton<
    IDatasetLoader,
    DatasetLoader>();

builder.Services.AddSingleton<
    IKnnSearchService,
    KnnSearchService>();

builder.Services.AddSingleton<
    IFraudScoringService,
    FraudScoringService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();