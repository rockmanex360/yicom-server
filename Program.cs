using Server.Models;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IReceiver, Receiver>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("null")
                .AllowCredentials();
        });
});

builder.WebHost.UseUrls("http://localhost:3000");

var app = builder.Build();

app.UseCors();

app.UseRouting();
app.MapHub<MessageHub>("/send");

var receiver = app.Services.GetRequiredService<IReceiver>();
receiver.Receive();

app.MapGet("/", () => "Hello World!");

app.Run();
