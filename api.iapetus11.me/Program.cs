using api.iapetus11.me.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IMinecraftServerService, MinecraftServerService>();
builder.Services.AddLazyCache();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();