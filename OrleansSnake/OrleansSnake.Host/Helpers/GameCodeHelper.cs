using System.Text;

namespace OrleansSnake.Host.Helpers;

public class GameCodeHelper
{
    private static readonly char[] _characters = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".ToCharArray();

    public string GenerateGameCode()
    {
        var gameCodeBuilder = new StringBuilder(6);

        for (int i = 0; i < gameCodeBuilder.Capacity; i++)
        {
            gameCodeBuilder.Append(_characters[Random.Shared.Next(_characters.Length)]);
        }

        return gameCodeBuilder.ToString();
    }
}