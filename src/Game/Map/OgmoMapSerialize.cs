using System.Numerics;

public class LevelData
{
    public int width;
    public int height;
    public List<LayerData>? layers;
}

public class LayerData
{
    public string? name;
    public string? tileset;
    public int gridCellsX;
    public int gridCellsY;
    public int gridCellWidth;
    public int gridCellHeight;
    public List<int>? data;
    public List<EntityData>? entities;

    public Vector2 ParsePosition(int index)
    {
        var x = index % gridCellsX;
        var y = gridCellsY - (index / gridCellsX);
        var tilePosition = new Vector2(x * gridCellWidth, y * gridCellHeight);
        return tilePosition;
    }
}

public class EntityData
{
    public string? name;
    public int x;
    public int y;
    public Dictionary<string, string>? values;
    public List<Vector2Serialize>? nodes;
}

public class Vector2Serialize
{
    public int x;
    public int y;
}