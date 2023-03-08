using OrleansSnake.Host;
using OrleansSnake.Host.Helpers;
using OrleansSnake.Host.Hubs;
using OrleansSnake.Host.Managers;
using OrleansSnake.Host.Workers;
using OrleansSnake.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orleans.Configuration;
using System.Net;


// Web Application Builder
var builder = WebApplication.CreateBuilder(args);


// Configuration
builder.Configuration.AddEnvironmentVariables();


// Dependency Injection
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<GameCodeHelper>();
builder.Services.AddTransient<GameManager>();
builder.Services.AddTransient<IApiHelper, ApiHelper>();
builder.Services.AddTransient(typeof(IApiHelper<>), typeof(ApiHelper<>));
builder.Services.AddSignalR();
builder.Services.AddSingleton<TickerHub>();
builder.Services.AddHostedService<TickerWorker>();


// Orleans Hosting
builder.Host.UseOrleans((hostBuilder, siloBuilder) =>
{
    siloBuilder.UseLocalhostClustering(siloPort: 11112, gatewayPort: 30001, primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, 11112), serviceId: "orleans-snake-host", clusterId: "orleans-snake-host");

    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "orleans-snake-host";
        options.ServiceId = "orleans-snake-host";
    });

    siloBuilder.ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
    });

    siloBuilder.UseDashboard();
});

var app = builder.Build();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Map HTTP endpoints using minimal API's
app.MapGet("/status",
    (IApiHelper helper) => helper.Execute());

app.MapPost("/games",
    (IApiHelper<GameManager> helper, CreateGameRequest request) => helper.Post(l => l.CreateGame(request)));

app.MapPost("/games/{gameCode}/join",
    (IApiHelper<GameManager> helper, [FromRoute] string gameCode, JoinGameRequest request) => helper.Post(l => l.JoinGame(request with { GameCode = gameCode })));

app.MapPost("/games/{gameCode}/ready/{playerName}",
    (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.ReadyPlayer(new ReadyPlayerRequest(gameCode, playerName))));

app.MapPost("/games/{gameCode}/abandon/{playerName}",
    (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.Abandon(new AbandonRequest(gameCode, playerName))));

app.MapGet("/games/active",
    (IApiHelper<GameManager> helper) => helper.Execute(l => l.GetActiveGames()));


// Map the SignalR Hub
app.MapHub<TickerHub>("/ticker");


// Run the web application
await app.RunAsync();