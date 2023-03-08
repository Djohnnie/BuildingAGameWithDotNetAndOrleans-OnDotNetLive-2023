using System.Text.Json;

namespace OrleansSnake.Contracts;

[GenerateSerializer]
public struct Food
{
    [Id(0)]
    public List<Bite> Bites { get; set; }

    public string ToFoodData()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Food FromFoodData(string foodData)
    {
        return JsonSerializer.Deserialize<Food>(foodData);
    }
}

[GenerateSerializer]
public struct Bite
{
    [Id(0)]
    public int X { get; set; }

    [Id(1)]
    public int Y { get; set; }
}

[GenerateSerializer]
public class Game
{
    [Id(0)]
    public string Code { get; set; }

    [Id(1)]
    public int Width { get; set; }

    [Id(2)]
    public int Height { get; set; }

    [Id(3)]
    public bool IsReady { get; set; }

    [Id(4)]
    public bool IsActive { get; set; }

    [Id(5)]
    public string FoodData { get; set; }

    [Id(6)]
    public List<Player> Players { get; set; }
}

[GenerateSerializer]
public class Player
{
    [Id(0)]
    public string Name { get; set; }

    [Id(1)]
    public bool IsReady { get; set; }

    [Id(2)]
    public string SnakeData { get; set; }
}

[GenerateSerializer]
public struct Snake
{
    private static Random _random = new();

    [Id(0)]
    public int Points { get; set; }

    [Id(1)]
    public Orientation Orientation { get; set; }

    [Id(2)]
    public List<Coordinates> Coordinates { get; set; }

    public string ToSnakeData()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Snake FromSnakeData(string snakeData)
    {
        return JsonSerializer.Deserialize<Snake>(snakeData);
    }

    public static Snake RandomSnake(int width, int height, int length)
    {
        var snake = new Snake
        {
            Points = 0,
            Orientation = (Orientation)_random.Next(4),
            Coordinates = new List<Coordinates>()
        };

        var randomX = _random.Next(length, width - length);
        var randomY = _random.Next(length, height - length);

        switch (snake.Orientation)
        {
            case Orientation.North:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX, Y = randomY + i });
                }
                break;
            case Orientation.East:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX - i, Y = randomY });
                }
                break;
            case Orientation.South:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX, Y = randomY - i });
                }
                break;

            case Orientation.West:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX + i, Y = randomY });
                }
                break;
        }

        return snake;
    }
}


[GenerateSerializer]
public struct Coordinates
{
    [Id(0)]
    public int X { get; set; }

    [Id(1)] 
    public int Y { get; set; }
}

public enum Orientation
{
    North,
    East,
    South,
    West
}