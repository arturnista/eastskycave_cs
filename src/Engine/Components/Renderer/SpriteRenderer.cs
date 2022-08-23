using System.Numerics;
using Raylib_cs;

public class SpriteRenderer : IRenderer
{

    private Sprite _sprite;
    private Image _image;
    private Texture2D _texture;

    public bool FlipX { get; set; }
    public bool FlipY { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public Color Color { get; set; } = Color.WHITE;
    public int DrawOrder { get; set; } = 0;

    public SpriteRenderer(string spriteName)
    {
        _sprite = DI.Get<AssetLoader>().GetSprite(spriteName);
        _image = _sprite.Image;
        _texture = _sprite.Texture;
    }

    public void Draw(Vector2 position)
    {
        Vector2 normalizedPos = position;
        normalizedPos.X = (int)position.X;
        normalizedPos.Y = -(int)position.Y;

        float width = _image.width * Scale.X;
        float height = _image.height * Scale.Y;
        Raylib.DrawTexturePro(
            _texture,
            new Rectangle(
                0,
                0,
                _image.width * (FlipX ? -1 : 1),
                _image.height * (FlipY ? -1 : 1)
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