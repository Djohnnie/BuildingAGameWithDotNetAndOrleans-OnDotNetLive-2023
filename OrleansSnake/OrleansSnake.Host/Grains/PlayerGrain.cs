using OrleansSnake.Contracts;
using Orleans.Runtime;
using System.Numerics;

namespace OrleansSnake.Host.Grains;

public interface IPlayerGrain : IGrainWithStringKey
{
    Task SetPlayer(Player player);
    Task UpdateSnakeData(string snakeData);
    Task<Orientation> GetOrientation();
    Task Abandon();
    Task<bool> IsReady();
    Task SetOrientation(Orientation orientation);
    Task<Player> GetPlayer();
    Task ReadyPlayer();
}

public class PlayerState
{
    public string Name { get; set; }
    public bool IsReady { get; set; }
    public string SnakeData { get; set; }
}

public class PlayerGrain : Grain, IPlayerGrain
{
    private readonly PlayerState _state = new PlayerState();

    public async Task SetPlayer(Player player)
    {
        _state.Name = player.Name;
        _state.SnakeData = player.SnakeData;
    }

    public Task UpdateSnakeData(string snakeData)
    {
        _state.SnakeData = snakeData;

        return Task.CompletedTask;
    }

    public Task<Orientation> GetOrientation()
    {
        return Task.FromResult(Snake.FromSnakeData(_state.SnakeData).Orientation);
    }

    public async Task Abandon()
    {
        DeactivateOnIdle();
    }

    public Task<bool> IsReady()
    {
        return Task.FromResult(_state.IsReady);
    }

    public async Task SetOrientation(Orientation orientation)
    {
        var snake = Snake.FromSnakeData(_state.SnakeData);
        snake.Orientation = orientation;
        _state.SnakeData = snake.ToSnakeData();
    }

    public Task<Player> GetPlayer()
    {
        return Task.FromResult(new Player
        {
            Name = _state.Name,
            IsReady = _state.IsReady,
            SnakeData = _state.SnakeData
        });
    }

    public Task ReadyPlayer()
    {
        _state.IsReady = true;

        return Task.CompletedTask;
    }
}