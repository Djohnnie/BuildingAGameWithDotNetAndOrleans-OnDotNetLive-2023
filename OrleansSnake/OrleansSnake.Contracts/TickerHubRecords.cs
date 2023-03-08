using Orleans;

namespace OrleansSnake.Contracts;

[GenerateSerializer]
public class GameState
{
    [Id(0)]
    public int Width { get; set; }

    [Id(1)]
    public int Height { get; set; }

    [Id(2)]
    public bool IsReady { get; set; }

    [Id(3)]
    public string GameCode { get; set; }

    [Id(4)]
    public List<PlayerState> Players { get; set; }

    [Id(5)]
    public Food Food { get; set; }
}

[GenerateSerializer]
public class PlayerState
{
    [Id(0)]
    public string PlayerName { get; set; }

    [Id(1)]
    public bool IsReady { get; set; }

    [Id(2)]
    public Snake Snake { get; set; }
}