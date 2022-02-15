using api.iapetus11.me.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddLazyCache();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddSingleton<IMinecraftImageService, MinecraftImageService>();

builder.Services.AddSingleton<IRedditPostFetcher, RedditPostFetcher>();
builder.Services.AddHostedService<IRedditPostFetcher>(provider => provider.GetService<IRedditPostFetcher>());

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.iapetus11.me v1.0.0"));
}

app.Run();