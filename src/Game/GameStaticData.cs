public static class GameStaticData
{
    // Tile size
    public const int TileSize = 8;
    public static int WindowWidth;
    public static int WindowHeight;
    public static int Zoom;

    // Physics
    public static float Gravity => 30f * GameStaticData.TileSize;
}