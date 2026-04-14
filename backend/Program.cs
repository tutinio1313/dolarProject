using System.Net;
using System.Text.Json;
using backend.database;
using backend.database.repository.currency;
using backend.Middleware;
using common.model;
using backend.services.instances;
using backend.services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(builder.Configuration["http_port"]!));
    options.ListenAnyIP(int.Parse(builder.Configuration["https_port"]!)
    ,options =>
    {
        options.UseHttps( builder.Configuration["ASPNET_CREDENTIAL_FILE_DEFAULT"]!, builder.Configuration["ASPNET_CREDENTIAL_FILE_PASSWORD"]!);
    });
});

builder.Services.AddDbContext<ContainerDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration["CONNECTION_STRING"]);
    }
);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

builder.Services.AddHostedService<ApiWorker>();


builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int) HttpStatusCode.PermanentRedirect;
    options.HttpsPort = int.Parse(builder.Configuration["https_port"]!);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

await ApplyMigration.Apply(app);
await ApplyMigration.PopulateData(
    builder.Configuration, 
app.Services.CreateScope().ServiceProvider.GetRequiredService<IApiService>(),
app.Services.CreateScope().ServiceProvider.GetRequiredService<ICurrencyRepository>()
);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseHsts();
app.UseRouting();
app.MapControllers();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/dolar", async (ICurrencyRepository repo, IApiService apiService) => await repo.GetLatest())
.WithName("Dolar");

app.MapGet("/ping", async () =>
{
    return "pong uwu kawaiiiiiiii";
});

app.Run();