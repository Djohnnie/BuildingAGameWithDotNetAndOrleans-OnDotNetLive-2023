using OrleansSnake.Host.Hubs;
using OrleansSnake.Host.Managers;
using OrleansSnake.Contracts;
using System.Diagnostics;

namespace OrleansSnake.Host.Workers;

public class TickerWorker : BackgroundService
{
    private readonly TickerHub _tickerHub;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TickerWorker(
        TickerHub tickerHub,
        IServiceScopeFactory serviceScopeFactory)
    {
        _tickerHub = tickerHub;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5000);

        while (!stoppingToken.IsCancellationRequested)
        {
            var stopWatch = Stopwatch.StartNew();

            using var serviceScope = _serviceScopeFactory.CreateScope();
            var gameManager = serviceScope.ServiceProvider.GetService<GameManager>();
            var activeGames = await gameManager.GetActiveGames();

            foreach (var activeGame in activeGames.ActiveGames)
            {
                var isReady = activeGame.IsReady || activeGame.Players.All(x => x.IsReady);

                var gameState = new GameState
                {
                    Width = activeGame.Width,
                    Height = activeGame.Height,
                    IsReady = isReady,
                    GameCode = activeGame.GameCode,
                    Players = activeGame.Players.Select(x => new PlayerState
                    {
                        PlayerName = x.PlayerName,
                        IsReady = x.IsReady,
                        Snake = Snake.FromSnakeData(x.SnakeData)
                    }).ToList(),
                    Food = Food.FromFoodData(activeGame.FoodData)
                };

                await _tickerHub.SendGameState(gameState);
            }

            stopWatch.Stop();
            await Task.Delay(Math.Max(0, 250 - (int)stopWatch.ElapsedMilliseconds), stoppingToken);
        }
    }
}