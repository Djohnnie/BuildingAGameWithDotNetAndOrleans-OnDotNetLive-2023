using OrleansSnake.Contracts;

namespace OrleansSnake.Client.Renderers;

public static class SnakeRenderer
{
    private static Color Snake1Color = Color.FromArgb(255, 0, 0);
    private static Brush Snake1Brush = new SolidBrush(Snake1Color);
    private static Color Snake2Color = Color.FromArgb(0, 255, 0);
    private static Brush Snake2Brush = new SolidBrush(Snake2Color);
    private static Color Snake3Color = Color.FromArgb(0, 0, 255);
    private static Brush Snake3Brush = new SolidBrush(Snake3Color);
    private static Color Snake4Color = Color.FromArgb(255, 0, 255);
    private static Brush Snake4Brush = new SolidBrush(Snake4Color);
    private static Color Snake5Color = Color.FromArgb(0, 255, 255);
    private static Brush Snake5Brush = new SolidBrush(Snake5Color);
    private static Color Snake6Color = Color.FromArgb(0, 0, 0);
    private static Brush Snake6Brush = new SolidBrush(Snake6Color);

    private static Brush[] SnakeBrushes = { Snake1Brush, Snake2Brush, Snake3Brush, Snake4Brush, Snake5Brush, Snake6Brush };

    public static void RenderSnakes(this Graphics g, Rectangle bounds, GameState gameState)
    {
        int pixelsPerTileX = bounds.Width / gameState.Width;
        int pixelsPerTileY = bounds.Height / gameState.Height;
        int pixelsPerTile = Math.Min(pixelsPerTileX, pixelsPerTileY) - 1;

        int offsetX = (bounds.Width - pixelsPerTile * gameState.Width) / 2;
        int offsetY = (bounds.Height - pixelsPerTile * gameState.Height) / 2;

        for (int y = 0; y < gameState.Height; y++)
        {
            for (int x = 0; x < gameState.Width; x++)
            {
                var tileBounds = new Rectangle(offsetX + pixelsPerTile * x, offsetY + pixelsPerTile * y, pixelsPerTile, pixelsPerTile);

                for (int p = 0; p < gameState.Players.Count; p++)
                {
                    var player = gameState.Players[p];

                    if (player.IsReady)
                    {
                        foreach (var coordinate in player.Snake.Coordinates)
                        {
                            if (coordinate.X == x && coordinate.Y == y)
                            {
                                g.FillRectangle(SnakeBrushes[p], tileBounds);
                            }
                        }
                    }
                }
            }
        }
    }
}