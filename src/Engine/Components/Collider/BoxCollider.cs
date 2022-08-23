using System.Numerics;
using Raylib_cs;

public struct Bounds
{
    public int MinX;
    public int MinY;
    public int MaxX;
    public int MaxY;
}

public class BoxCollider
{
    private Rectangle _rec;
    private Bounds _bounds = new Bounds();
    public Rectangle Rec => _rec;

    public BoxCollider(float width, float height) : this(new Rectangle(0.5f, 0.5f, width, height))
    {
        
    }

    public BoxCollider(Rectangle rec)
    {
        _rec = rec;
    }

    public Bounds GetBounds(Vector2 position, bool debug = false)
    {
        // _bounds.MinX = (int)(position.X - _rec.width / 2);
        // _bounds.MinY = (int)(position.Y - _rec.height / 2);
        // _bounds.MaxX = (int)(position.X + _rec.width / 2);
        // _bounds.MaxY = (int)(position.Y + _rec.height / 2);

        _bounds.MaxX = (int)(position.X + Math.Round(_rec.width * _rec.x));
        _bounds.MaxY = (int)(position.Y + Math.Round(_rec.height * _rec.y));
        _bounds.MinX = (int)(position.X - Math.Round(_rec.width * _rec.x));
        _bounds.MinY = (int)(position.Y - Math.Round(_rec.height * _rec.y));

        if (debug) Console.WriteLine($"MinX: {_bounds.MinX} = (int)({position.X} - {_rec.width / 2})");
        if (debug) Console.WriteLine($"MinY: {_bounds.MinY} = (int)({position.Y} - {_rec.height / 2})");
        if (debug) Console.WriteLine($"MaxX: {_bounds.MaxX} = (int)({position.X} + {_rec.width / 2})");
        if (debug) Console.WriteLine($"MaxY: {_bounds.MaxY} = (int)({position.Y} + {_rec.height / 2})");

        return _bounds;
    }
}