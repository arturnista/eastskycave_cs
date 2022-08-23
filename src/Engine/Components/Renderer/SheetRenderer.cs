using System.Numerics;
using Raylib_cs;

public class SheetRenderer : IRenderer
{
    private string _spriteName;
    private Spritesheet _sprite;
    private Image _image;
    private Texture2D _texture;
    private int _xIndex;
    private int _yIndex;

    public bool FlipX { get; set; }
    public bool FlipY { get; set; }
    public float Rotation { get; set; }
    public int DrawOrder { get; set; } = 0;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Color Color { get; set; } = Color.WHITE;

    public SheetRenderer(string spriteName, int index)
    {
        _spriteName = spriteName;
        _sprite = DI.Get<AssetLoader>().GetSprite(spriteName) as Spritesheet;
        _image = _sprite.Image;
        _texture = _sprite.Texture;
        SetIndex(index);
    }

    public void SetIndex(int index)
    {
        int spritesHorizontal = _image.width / _sprite.TileWidth;
        int spritesVertical = _image.height / _sprite.TileHeight;
        _xIndex = index % spritesHorizontal;
        _yIndex = index / spritesHorizontal;
    }

    public void Draw(Vector2 position)
    {
        Vector2 normalizedPos = position;
        normalizedPos.X = (int)position.X;
        normalizedPos.Y = -(int)position.Y;

        float width = _sprite.TileWidth * Scale.X;
        float height = _sprite.TileHeight * Scale.Y;
        Raylib.DrawTexturePro(
            _texture,
            new Rectangle(
                _xIndex * _sprite.TileWidth,
                _yIndex * _sprite.TileHeight,
                _sprite.TileWidth * (FlipX ? -1 : 1),
                _sprite.TileHeight * (FlipY ? -1 : 1)
            ),
            new Rectangle(
                normalizedPos.X,
                normalizedPos.Y,
                width,
                height
            ),
            new Vector2(
                width / 2,
                height / 2
            ),
            Rotation,
            Color
        );
    }

}