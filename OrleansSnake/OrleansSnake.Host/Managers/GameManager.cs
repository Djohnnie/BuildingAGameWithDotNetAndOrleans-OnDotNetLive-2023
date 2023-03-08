using OrleansSnake.Host.Grains;
using OrleansSnake.Host.Helpers;
using OrleansSnake.Contracts;

namespace OrleansSnake.Host.Managers;

public class GameManager
{
    public const int SnakeGameWidth = 30;
    public const int SnakeGameHeight = 16;
    public const int SnakeLength = 5;

    private readonly IGrainFactory _grainFactory;
    private readonly GameCodeHelper _gameCodeHelper;

    public GameManager(
        IGrainFactory grainFactory,
        GameCodeHelper gameCodeHelper)
    {
        _grainFactory = grainFactory;
        _gameCodeHelper = gameCodeHelper;
    }

    public async Task<CreateGameResponse> CreateGame(CreateGameRequest request)
    {
        var gameCode = string.Empty;

        var gamesGrain = _grainFactory.GetGrain<IGamesGrain>(Guid.Empty);

        do
        {
            gameCode = _gameCodeHelper.GenerateGameCode();
        }
        while (await gamesGrain.GameCodeExists(gameCode));

        var player = new Player
        {
            Name = request.HostPlayerName,
            SnakeData = Snake.RandomSnake(SnakeGameWidth, SnakeGameHeight, SnakeLength).ToSnakeData()
        };

        await gamesGrain.CreateGame(gameCode, player);

        return new CreateGameResponse(gameCode);
    }

    public async Task<JoinGameResponse?> JoinGame(JoinGameRequest request)
    {
        var gamesGrain = _grainFactory.GetGrain<IGamesGrain>(Guid.Empty);
        var gameCode = await gamesGrain.GetGame(request.GameCode);

        if (gameCode == null)
        {
            return null;
        }

        var player = new Player
        {
            Name = request.PlayerName,
            SnakeData = Snake.RandomSnake(SnakeGameWidth, SnakeGameHeight, SnakeLength).ToSnakeData()
        };

        var gameGrain = _grainFactory.GetGrain<IGameGrain>(gameCode);
        await gameGrain.AddPlayer(player);

        return new JoinGameResponse(gameCode);
    }

    public async Task<ReadyPlayerResponse> ReadyPlayer(ReadyPlayerRequest request)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameCode);
        await gameGrain.ReadyPlayer(request.PlayerName);

        return new ReadyPlayerResponse();
    }

    public async Task<GetActiveGamesResponse> GetActiveGames()
    {
        var gamesGrain = _grainFactory.GetGrain<IGamesGrain>(Guid.Empty);
        var activeGames = await gamesGrain.GetActiveGames();

        return new GetActiveGamesResponse(
            activeGames.Select(x => new ActiveGame(x.Code, x.Width, x.Height, x.IsReady, x.Players.Select(p => new ActivePlayer(p.Name, p.IsReady, p.SnakeData)).ToList(), x.FoodData)).ToList());
    }

    public async Task<AbandonResponse> Abandon(AbandonRequest request)
    {
        var gamesGrain = _grainFactory.GetGrain<IGamesGrain>(Guid.Empty);
        await gamesGrain.AbandonPlayer(request.GameCode, request.PlayerName);

        return new AbandonResponse();
    }
}