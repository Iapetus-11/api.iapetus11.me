using api.iapetus11.me.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();