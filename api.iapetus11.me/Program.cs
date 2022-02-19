using api.iapetus11.me.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, config) => config.AddJsonFile("secrets.json"));

builder.Services.AddControllers();

builder.Services.AddLazyCache();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddScoped<IMinecraftImageService, MinecraftImageService>();

builder.Services.AddSingleton<IRedditPostFetcher, RedditPostFetcher>();
builder.Services.AddHostedService<IRedditPostFetcher>(provider => provider.GetService<IRedditPostFetcher>());

var staticAssetsService = new StaticAssetsService();
staticAssetsService.CacheAllAssets();
builder.Services.AddSingleton<IStaticAssetsService>(staticAssetsService);

// add seq logging
if (builder.Environment.IsProduction())
{
    var seqConfig = builder.Configuration.GetSection("Seq");
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSeq(serverUrl: seqConfig["Url"], apiKey: seqConfig["Key"]));
}
else
{
    builder.Services.AddLogging();
}

var app = builder.Build();

app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.iapetus11.me v1.0.0"));
}

app.MapGet("/", () => new
{
    Author = "Iapetus11 / Milo Weinberg",
    Repository = "https://github.com/Iapetus-11/api.iapetus11.me"
});

app.Run();