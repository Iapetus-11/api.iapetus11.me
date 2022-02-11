using api.iapetus11.me.Services.Minecraft;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IServerStatusService, ServerStatusService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();