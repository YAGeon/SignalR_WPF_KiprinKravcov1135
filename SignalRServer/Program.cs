using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SignalRServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Rooms>();
builder.Services.AddSignalR().
    AddJsonProtocol(s =>
    {
        s.PayloadSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

app.MapHub<MyHub>("/game");

app.Run();
