using api.iapetus11.me.Services;
using Flurl.Http;
using Serilog;
using Serilog.Events;

Serilog.Debugging.SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json");

builder.Host.UseSerilog((host, provider, config) =>
{
    config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning).WriteTo.Console();
    
    if (builder.Environment.IsProduction())
    {
        var seqConfig = builder.Configuration.GetSection("Seq");
        config.WriteTo.Seq(seqConfig["Url"]!, apiKey: seqConfig["Key"]!);
    }
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        });
});

builder.Services.AddLazyCache();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFlurlClient, FlurlClient>();

builder.Services.AddScoped<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddScoped<IMinecraftImageService, MinecraftImageService>();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<IFractalsService, FractalsService>();

builder.Services.AddSingleton<IRedditPostService, RedditPostService>();
builder.Services.AddHostedService<IRedditPostService>(provider => provider.GetService<IRedditPostService>()!);

builder.Services.AddSingleton<IStaticAssetsService, StaticAssetsService>();
builder.Services.AddSingleton<ICacheTrackerService, CacheTrackerService>();
builder.Services.AddSingleton<IRedditAuthService, RedditAuthService>();

builder.Services.AddControllers();

var app = builder.Build();

app.Services.GetService<IStaticAssetsService>()!.CacheAllAssets();

app.UseRouting();
app.UseCors();
app.MapControllers();

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

app.Services.GetService<ILogger<object>>()!.LogInformation(
    "Starting up api.iapetus11.me ({env})!", builder.Environment.IsProduction() ? "prod" : "dev");

app.Run();
