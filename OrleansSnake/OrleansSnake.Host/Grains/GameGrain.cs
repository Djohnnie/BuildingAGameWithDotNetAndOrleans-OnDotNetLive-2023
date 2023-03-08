using OrleansSnake.Contracts;
using OrleansSnake.Host.Managers;

namespace OrleansSnake.Host.Grains;

public interface IGameGrain : IGrainWithStringKey
{
    Task CreateGame();
    Task AddPlayer(Player player);
    Task<Game> GetGame();
    Task ReadyPlayer(string playerName);
    Task UpdateFood(string foodData);
    Task UpdatePlayerStates(List<Contracts.PlayerState> playerStates);
    Task<bool> AbandonPlayer(string playerName);
    Task<Orientation?> GetPlayerOrientation(string playerName);
}

public class GameState
{
    public string Code { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsReady { get; set; }
    public bool IsActive { get; set; }
    public string FoodData { get; set; }
    public List<string> PlayerNames { get; set; }
}

public class GameGrain : Grain, IGameGrain
{
    private readonly GameState _state = new GameState();

    public Task CreateGame()
    {
        _state.Code = this.GetPrimaryKeyString();
        _state.Width = GameManager.SnakeGameWidth;
        _state.Height = GameManager.SnakeGameHeight;
        _state.IsReady = false;
        _state.IsActive = true;
        _state.FoodData = new Food { Bites = new List<Bite>() }.ToFoodData();
        _state.PlayerNames = new List<string>();

        return Task.CompletedTask;
    }

    public async Task AddPlayer(Player player)
    {
        var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(player.Name);
        await playerGrain.SetPlayer(player);

        _state.PlayerNames.Add(player.Name);
    }

    public async Task<Game> GetGame()
    {
        var gameCode = this.GetPrimaryKeyString();
        var processingGrain = GrainFactory.GetGrain<IProcessingGrain>(gameCode);
        await processingGrain.Ping();
        
        var players = new List<Player>();

        foreach (var playerName in _state.PlayerNames)
        {
            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(playerName);
            var player = await playerGrain.GetPlayer();
            players.Add(player);
        }

        return new Game
        {
            Code = _state.Code,
            Width = _state.Width,
            Height = _state.Height,
            IsReady = _state.IsReady,
            IsActive = _state.IsActive,
            FoodData = _state.FoodData,
            Players = players
        };
    }

    public async Task ReadyPlayer(string playerName)
    {
        if (!_state.PlayerNames.Contains(playerName))
        {
            return;
        }

        var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(playerName);
        await playerGrain.ReadyPlayer();

        var game = await GetGame();

        if (game.Players.All(p => p.IsReady))
        {
            await ReadyGame();
        }
    }

    public async Task UpdateFood(string foodData)
    {
        _state.FoodData = foodData;
    }

    public async Task UpdatePlayerStates(List<Contracts.PlayerState> playerStates)
    {
        foreach (var playerState in playerStates)
        {
            if (_state.PlayerNames.Contains(playerState.PlayerName))
            {
                var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(playerState.PlayerName);
                await playerGrain.UpdateSnakeData(playerState.Snake.ToSnakeData());
            }
        }
    }

    public async Task<bool> AbandonPlayer(string playerName)
    {
        if (_state.PlayerNames.Contains(playerName))
        {
            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(playerName);
            await playerGrain.Abandon();
            _state.PlayerNames.Remove(playerName);

            if (_state.PlayerNames.Count == 0)
            {
                _state.IsActive = false;

                var gameCode = this.GetPrimaryKeyString();
                var processingGrain = GrainFactory.GetGrain<IProcessingGrain>(gameCode);
                await processingGrain.Abandon();

                DeactivateOnIdle();
            }
        }

        return _state.IsActive;
    }

    public async Task<Orientation?> GetPlayerOrientation(string playerName)
    {
        if (_state.PlayerNames.Contains(playerName))
        {
            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(playerName);
            return await playerGrain.GetOrientation();
        }

        return null;
    }

    private async Task ReadyGame()
    {
        _state.IsReady = true;

        var gameCode = this.GetPrimaryKeyString();
        var processingGrain = GrainFactory.GetGrain<IProcessingGrain>(gameCode);
        await processingGrain.Ping();
    }
}