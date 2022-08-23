using Raylib_cs;
using System.Numerics;

public interface IRenderer
{
    Vector2 Scale { get; set; }
    float Rotation { get; set; }
    bool FlipX { get; set; }
    bool FlipY { get; set; }
    int DrawOrder { get; set; }
    Color Color { get; set; }

    void Draw(Vector2 position);
}