using Raylib_cs;

public class Sprite
{
    public Image Image { get; }
    public Texture2D Texture { get; }

    public Sprite(Image image, Texture2D texture)
    {
        Image = image;
        Texture = texture;
    }
}

public class Spritesheet : Sprite
{
    
    public int TileWidth { get; }
    public int TileHeight { get; }

    public Spritesheet(Image image, Texture2D texture, int tileWidth, int tileHeight) : base(image, texture)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;
    }
}

public class AssetLoader
{

    public Dictionary<string, Sprite> Sprites { get; private set; } = new Dictionary<string, Sprite>();

    public AssetLoader()
    {

    }

    public void LoadSprite(string name, string path)
    {
        var image = Raylib.LoadImage(path);
        var texture = Raylib.LoadTextureFromImage(image);
        Sprites.Add(name, new Sprite(image, texture));
    }

    public void LoadSpritesheet(string name, string path, int tileWidth, int tileHeight)
    {
        var image = Raylib.LoadImage(path);
        var texture = Raylib.LoadTextureFromImage(image);
        Sprites.Add(name, new Spritesheet(image, texture, tileWidth, tileHeight));
    }

    public Sprite GetSprite(string name)
    {
        return Sprites[name];
    }
       
}