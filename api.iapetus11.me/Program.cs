using api.iapetus11.me.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, config) => config.AddJsonFile("secrets.json"));

builder.Host.UseSerilog((host, provider, config) =>
{
    config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning).WriteTo.Console();
    
    if (builder.Environment.IsProduction())
    {
        var seqConfig = builder.Configuration.GetSection("Seq");
        config.WriteTo.Seq(seqConfig["Url"], apiKey: seqConfig["Key"]);
    }
});

builder.Services.AddControllers();

builder.Services.AddLazyCache();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddScoped<IMinecraftImageService, MinecraftImageService>();

builder.Services.AddSingleton<IRedditPostFetcher, RedditPostFetcher>();
builder.Services.AddHostedService<IRedditPostFetcher>(provider => provider.GetService<IRedditPostFetcher>()!);

builder.Services.AddSingleton<IStaticAssetsService, StaticAssetsService>();

var app = builder.Build();

app.Services.GetService<IStaticAssetsService>()!.CacheAllAssets();

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