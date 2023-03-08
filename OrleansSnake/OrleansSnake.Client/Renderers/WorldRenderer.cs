using OrleansSnake.Contracts;

namespace OrleansSnake.Client.Renderers;

public static class WorldRenderer
{
    public static Color WorldColor = Color.FromArgb(255, 255, 255);
    public static Color PenColor = Color.FromArgb(200, 200, 200);
    public static Brush TileBrush = new SolidBrush(WorldColor);
    public static Pen TilePen = new Pen(PenColor);

    public static void RenderWorld(this Graphics g, Rectangle bounds, GameState gameState)
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
                g.FillRectangle(TileBrush, tileBounds);
                g.DrawRectangle(TilePen, tileBounds);
            }
        }
    }
}