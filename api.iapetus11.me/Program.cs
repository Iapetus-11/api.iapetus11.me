using api.iapetus11.me.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, config) => config.AddJsonFile("secrets.json"));

builder.Services.AddControllers();

builder.Services.AddLazyCache();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();

// add logging
if (builder.Environment.IsProduction())
{
    // logging with datalust seq
    var seqConfig = builder.Configuration.GetSection("Seq");
    builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSeq(seqConfig["Url"], seqConfig["Key"]));
}
else
{
    builder.Services.AddLogging();
}

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