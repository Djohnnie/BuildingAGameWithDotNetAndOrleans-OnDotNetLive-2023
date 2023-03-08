using OrleansSnake.Contracts;
using System.Net.Http.Json;

namespace OrleansSnake.Client;

internal class GameClient
{
    private static string Host = "https://localhost:7243";

    public async Task<string> CreateGame(string hostPlayerName)
    {
        var request = new CreateGameRequest(hostPlayerName);

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"{Host}/games", request);
        var createGameResponse = await response.Content.ReadFromJsonAsync<CreateGameResponse>();

        return createGameResponse.GameCode;
    }

    public async Task<string> JoinGame(string gameCode, string playerName)
    {
        var request = new JoinGameRequest(gameCode, playerName);

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"{Host}/games/{gameCode}/join", request);
        var joinGameResponse = await response.Content.ReadFromJsonAsync<JoinGameResponse>();

        return joinGameResponse.GameCode;
    }

    public async Task PlayerReady(string gameCode, string playerName)
    {
        var client = new HttpClient();
        _ = await client.PostAsJsonAsync($"{Host}/games/{gameCode}/ready/{playerName}", "null");
    }

    public async Task Abandon(string gameCode, string playerName)
    {
        var client = new HttpClient();
        _ = await client.PostAsJsonAsync($"{Host}/games/{gameCode}/abandon/{playerName}", "null");
    }
}