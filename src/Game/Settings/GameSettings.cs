public struct GameSettings
{
    public GeneralSettings general;
    public GameplaySettings gameplay;
}

public struct GeneralSettings
{
    public string name;
    public int screenWidth;
    public int screenHeight;
    public int zoom;
}

public struct GameplaySettings
{
    public string initialLevel;
}