namespace OrleansSnake.Contracts;

public record CreateGameRequest(string HostPlayerName);
public record CreateGameResponse(string GameCode);

public record JoinGameRequest(string GameCode, string PlayerName);
public record JoinGameResponse(string GameCode);

public record ReadyPlayerRequest(string GameCode, string PlayerName);
public record ReadyPlayerResponse;

public record GetActiveGamesResponse(List<ActiveGame> ActiveGames);
public record ActiveGame(string GameCode, int Width, int Height, bool IsReady, List<ActivePlayer> Players, string FoodData);
public record ActivePlayer(string PlayerName, bool IsReady, string SnakeData);

public record AbandonRequest(string GameCode, string PlayerName);
public record AbandonResponse;