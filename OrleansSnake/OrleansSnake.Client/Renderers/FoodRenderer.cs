using OrleansSnake.Contracts;

namespace OrleansSnake.Client.Renderers;

public static class FoodRenderer
{
    private static Color FoodColor = Color.FromArgb(255, 175, 0);
    private static Brush FoodBrush = new SolidBrush(FoodColor);

    public static void RenderFood(this Graphics g, Rectangle bounds, GameState gameState)
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

                foreach (var bite in gameState.Food.Bites)
                {
                    if (bite.X == x && bite.Y == y)
                    {
                        g.FillEllipse(FoodBrush, tileBounds);
                    }
                }
            }
        }
    }
}