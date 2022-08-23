public struct AssetsSettings
{
    public Dictionary<string, ImageSettings> images;
    public Dictionary<string, string> levels;
}

public struct ImageSettings
{
    public bool isSheet;
    public string src;
    public int width;
    public int height;
}