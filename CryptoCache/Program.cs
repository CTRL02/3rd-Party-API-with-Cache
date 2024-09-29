using CryptoCache.Middleware;
using CryptoCache.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Headers;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "_Crypto";
});
builder.Services.AddScoped<IRedisCacheService, RedisCacherService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpsRedirection();

app.MapGet("/CryptoMap", async (string country , IRedisCacheService _cache) =>
{
    var Data = _cache.GetData<string>(country);
    if (Data is not null)
        return Results.Ok(Data);

    string apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");

    if (string.IsNullOrEmpty(apiKey))
    {
        return Results.Problem("API key not found in environment variables.");
    }

    var baseUrl = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{country}?unitGroup=metric&key={apiKey}&contentType=json";

    using (HttpClient client = new HttpClient())
    {

        try
        {
            HttpResponseMessage response = await client.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode(); 

            string responseBody = await response.Content.ReadAsStringAsync();
            _cache.SetData(country, responseBody);
            return Results.Ok(responseBody);
        }
        catch (HttpRequestException e)
        {
            return Results.Problem($"Error fetching data: {e.Message}");
        }

    }
}).WithOpenApi();




app.Run();

