using OrleansSnake.Contracts;
using Orleans.Runtime;

namespace OrleansSnake.Host.Grains;

public interface IGamesGrain : IGrainWithGuidKey
{
    Task<bool> GameCodeExists(string gameCode);
    Task CreateGame(string gameCode, Player player);
    Task<string?> GetGame(string gameCode);
    Task<List<Game>> GetActiveGames();
    Task AbandonPlayer(string gameCode, string playerName);
}

public class GamesState
{
    public List<string> GameCodes { get; set; } = new List<string>();
}

public class GamesGrain : Grain, IGamesGrain
{
    private readonly GamesState _state = new GamesState();

    public Task<bool> GameCodeExists(string gameCode)
    {
        return Task.FromResult(_state.GameCodes.Contains(gameCode));
    }

    public async Task CreateGame(string gameCode, Player player)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameCode);
        await gameGrain.CreateGame();
        _state.GameCodes.Add(gameCode);

        await gameGrain.AddPlayer(player);
    }

    public Task<string?> GetGame(string gameCode)
    {
        if (_state.GameCodes.Contains(gameCode))
        {
            return Task.FromResult((string?)gameCode);
        }

        return Task.FromResult((string?)null);
    }

    public async Task<List<Game>> GetActiveGames()
    {
        var activeGames = new List<Game>();

        foreach (var gameCode in _state.GameCodes)
        {
            var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameCode);
            var game = await gameGrain.GetGame();

            if (game.IsActive)
            {
                activeGames.Add(game);
            }
        }

        return activeGames;
    }

    public async Task AbandonPlayer(string gameCode, string playerName)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameCode);
        var isActive = await gameGrain.AbandonPlayer(playerName);

        if (!isActive)
        {
            _state.GameCodes.Remove(gameCode);

            if (_state.GameCodes.Count == 0)
            {
                DeactivateOnIdle();
            }
        }
    }
}