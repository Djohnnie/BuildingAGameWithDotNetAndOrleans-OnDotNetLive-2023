using Orleans.Placement;
using OrleansSnake.Contracts;
using OrleansSnake.Host.Managers;

namespace OrleansSnake.Host.Grains;

public interface IProcessingGrain : IGrainWithStringKey
{
    Task Ping();

    Task Abandon();
}

[PreferLocalPlacement]
public class ProcessingGrain : Grain, IProcessingGrain
{
    private IDisposable? _timer;

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _timer = RegisterTimer(OnTimer, null, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(250));

        return base.OnActivateAsync(cancellationToken);
    }

    public Task Ping()
    {
        return Task.CompletedTask;
    }

    public Task Abandon()
    {
        if (_timer != null)
        {
            _timer.Dispose();
        }

        DeactivateOnIdle();

        return Task.CompletedTask;
    }

    private async Task OnTimer(object arg)
    {
        var gameCode = this.GetPrimaryKeyString();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameCode);

        var game = await gameGrain.GetGame();

        if (game.IsReady)
        {
            var food = Food.FromFoodData(game.FoodData);

            if (food.Bites.Count == 0)
            {
                food = GenerateRandomFood(game, food);
                await gameGrain.UpdateFood(food.ToFoodData());
            }

            var newPlayerStates = new List<Contracts.PlayerState>();

            foreach (var playerState in game.Players)
            {
                var snake = Snake.FromSnakeData(playerState.SnakeData);

                bool shouldGrow = false;
                var bitesToRemove = new List<Bite>();

                if (food.Bites.Count > 0)
                {
                    foreach (var bite in food.Bites)
                    {
                        foreach (var coordinate in snake.Coordinates)
                        {
                            if (bite.X == coordinate.X && bite.Y == coordinate.Y)
                            {
                                shouldGrow = true;
                                bitesToRemove.Add(bite);
                                break;
                            }
                        }
                    }
                }

                if (shouldGrow)
                {
                    foreach (var biteToRemove in bitesToRemove)
                    {
                        food.Bites.Remove(biteToRemove);
                    }
                }

                var lastMinuteOrientation = await gameGrain.GetPlayerOrientation(playerState.Name);

                var newPlayerState = new Contracts.PlayerState
                {
                    PlayerName = playerState.Name,
                    IsReady = playerState.IsReady,
                    Snake = new Snake
                    {
                        Orientation = lastMinuteOrientation.Value,
                        Coordinates = snake.Coordinates,
                        Points = snake.Points
                    }
                };

                var newState = ProgressSnake(newPlayerState, game, shouldGrow);
                newPlayerStates.Add(newState);
            }

            await gameGrain.UpdateFood(food.ToFoodData());
            await gameGrain.UpdatePlayerStates(newPlayerStates);
        }
    }

    private Food GenerateRandomFood(Game activeGame, Food food)
    {
        bool collision = false;

        do
        {
            var randomX = Random.Shared.Next(0, GameManager.SnakeGameWidth);
            var randomY = Random.Shared.Next(0, GameManager.SnakeGameHeight);

            foreach (var bite in food.Bites)
            {
                if (randomX == bite.X && randomY == bite.Y)
                {
                    collision = true;
                    break;
                }
            }

            foreach (var player in activeGame.Players)
            {
                var snake = Snake.FromSnakeData(player.SnakeData);

                foreach (var coordinate in snake.Coordinates)
                {
                    if (randomX == coordinate.X && randomY == coordinate.Y)
                    {
                        collision = true;
                        break;
                    }
                }
            }

            if (!collision)
            {
                food.Bites.Add(new Bite { X = randomX, Y = randomY });
            }

        } while (collision);

        return food;
    }

    private Contracts.PlayerState ProgressSnake(Contracts.PlayerState playerState, Game game, bool shouldGrow)
    {
        var snake = playerState.Snake;

        Func<Coordinates, Coordinates> moveFunc = coordinates => coordinates;

        switch (snake.Orientation)
        {
            case Orientation.North:
                moveFunc = coordinates =>
                {
                    var newY = coordinates.Y - 1 < 0 ? game.Height - 1 : coordinates.Y - 1;
                    return coordinates with { Y = newY };
                };

                break;
            case Orientation.East:
                moveFunc = coordinates =>
                {
                    var newX = coordinates.X + 1 >= game.Width ? 0 : coordinates.X + 1;
                    return coordinates with { X = newX };
                };

                break;
            case Orientation.South:
                moveFunc = coordinates =>
                {
                    var newY = coordinates.Y + 1 >= game.Height ? 0 : coordinates.Y + 1;
                    return coordinates with { Y = newY };
                };

                break;
            case Orientation.West:
                moveFunc = coordinates =>
                {
                    var newX = coordinates.X - 1 < 0 ? game.Width - 1 : coordinates.X - 1;
                    return coordinates with { X = newX };
                };

                break;
        }

        var newCoordinates = new List<Coordinates>(snake.Coordinates.Count + (shouldGrow ? 1 : 0));

        for (int i = 0; i < snake.Coordinates.Count; i++)
        {
            if (i == 0)
            {
                newCoordinates.Add(moveFunc(snake.Coordinates[0]));
            }
            else
            {
                newCoordinates.Add(snake.Coordinates[i - 1]);
            }
        }

        if (shouldGrow)
        {
            newCoordinates.Add(snake.Coordinates[^1]);
        }

        return new Contracts.PlayerState
        {
            PlayerName = playerState.PlayerName,
            IsReady = playerState.IsReady,
            Snake = new Snake
            {
                Orientation = snake.Orientation,
                Coordinates = newCoordinates,
                Points = snake.Points
            }
        };
    }
}