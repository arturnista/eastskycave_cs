using System.IO;
using System.Linq;
using System.Numerics;
using Raylib_cs;
using Newtonsoft.Json;
using Engine.Objects;
using Engine;

public class OgmoLevel
{

    public Vector2 MinPosition;
    public Vector2 MaxPosition;

    public Dictionary<Vector2, int> TilesBG = new Dictionary<Vector2, int>();
    public Dictionary<Vector2, int> Tiles = new Dictionary<Vector2, int>();
    public List<LevelColliderItem> Colliders = new List<LevelColliderItem>();
    public List<OgmoMapEntity> Entities = new List<OgmoMapEntity>();
}

public class LevelColliderItem
{
    public Vector2 Position;
    public int Type;
}

public class OgmoMapEntity
{
    public string? Name;
    public Vector2 Position;
    public Dictionary<string, string>? Params;
    public List<Vector2>? Nodes;
}

public class OgmoMap : IMap
{

    private Dictionary<string, OgmoLevel> _levels = new Dictionary<string, OgmoLevel>();
    private string _currentLevel;
    private List<IGameObject> _levelObjects = new List<IGameObject>();

    public OgmoLevel CurrentLevel => _levels[_currentLevel];

    public OgmoMap()
    {

    }

    public void LoadLevel(string levelName, string levelPath)
    {
        var level = new OgmoLevel();
        _levels.Add(levelName, level);

        string path = Path.Combine(levelPath);
        string jsonData = System.IO.File.ReadAllText(path);
        LevelData? levelData = JsonConvert.DeserializeObject<LevelData>(jsonData);

        foreach (var layer in levelData.layers)
        {
            if (layer.name == "ground")
            {
                for (int index = 0; index < layer.data.Count; index++)
                {
                    var tileId = layer.data[index];
                    var tilePosition = layer.ParsePosition(index);
                    
                    if (index == 0)
                    {
                        level.MinPosition = tilePosition;
                        level.MaxPosition = tilePosition;
                    }
                    else if (tilePosition.X <= level.MinPosition.X && tilePosition.Y <= level.MinPosition.Y)
                    {
                        level.MinPosition = tilePosition;
                    }
                    else if (tilePosition.X >= level.MaxPosition.X && tilePosition.Y >= level.MaxPosition.Y)
                    {
                        level.MaxPosition = tilePosition;
                    }

                    if (tileId == -1) continue;
                    level.Tiles.Add(tilePosition, tileId);
                }
            }
            else if (layer.name == "ground_bg")
            {
                for (int index = 0; index < layer.data.Count; index++)
                {
                    var tileId = layer.data[index];
                    if (tileId == -1) continue;

                    var tilePosition = layer.ParsePosition(index);
                    level.TilesBG.Add(tilePosition, tileId);
                }
            }
            else if (layer.name == "collider")
            {
                for (int index = 0; index < layer.data.Count; index++)
                {
                    var tileId = layer.data[index];
                    if (tileId == -1) continue;

                    var tilePosition = layer.ParsePosition(index);
                    level.Colliders.Add(new LevelColliderItem()
                    {
                        Position = tilePosition + new Vector2(-2f, 2f),
                        Type = tileId
                    });
                }
            }
            else if (layer.name == "entities")
            {
                for (int index = 0; index < layer.entities.Count; index++)
                {
                    EntityData entity = layer.entities[index];
                    var tilePosition = new Vector2(
                        entity.x,
                        (layer.gridCellsY - (entity.y / layer.gridCellHeight)) * layer.gridCellHeight
                    );
                    
                    if (entity.name == "player_spawn")
                    {
                        tilePosition.Y += 2;
                    }

                    level.Entities.Add(new OgmoMapEntity()
                    {
                        Position = tilePosition,
                        Name = entity.name,
                        Params = entity.values,
                        Nodes = entity.nodes?.Select(pos => new Vector2(pos.x, pos.y)).ToList(),
                    });
                }
            }
        }

    }

    public OgmoLevel Reset()
    {
        return ChangeLevel(_currentLevel);
    }

    public OgmoLevel ChangeLevel(string levelName)
    {
        _currentLevel = levelName;
        var objectSystem = DI.Get<ObjectSystem>();

        foreach (var obj in _levelObjects) objectSystem.Destroy(obj);
        _levelObjects.Clear();

        var tilesBG = _levels[levelName].TilesBG;
        foreach (var bg in tilesBG)
        {
            var pos = bg.Key;
            Decal ground = new Decal(pos);
            var renderer = new SheetRenderer("map_bg", bg.Value);
            renderer.DrawOrder = DrawableLayers.BackgroundLayer;
            ground.SetRenderer(renderer);
            objectSystem.Create(ground);
            _levelObjects.Add(ground);
        }

        var tiles = _levels[levelName].Tiles;
        foreach (var tile in tiles)
        {
            var pos = tile.Key;
            Decal ground = new Decal(pos);
            var renderer = new SheetRenderer("map", tile.Value);
            renderer.DrawOrder = DrawableLayers.MapLayer;
            ground.SetRenderer(renderer);
            objectSystem.Create(ground);
            _levelObjects.Add(ground);
        }

        var colliders = _levels[levelName].Colliders;
        foreach (var collider in colliders)
        {
            IGameObject gameObject;
            switch (collider.Type)
            {
                case 1:
                    gameObject = new DeathTrigger(collider.Position, new BoxCollider(4, 4));
                    break;
                default:
                    gameObject = new Solid(collider.Position, new BoxCollider(4, 4));
                    break;
            }
            objectSystem.Create(gameObject);
            _levelObjects.Add(gameObject);
        }
        
        var entities = _levels[levelName].Entities;
        foreach (var entity in entities)
        {
            if (entity.Name == "level_changer")
            {
                ChangeLevelTrigger trigger = new ChangeLevelTrigger(entity.Params["level_name"], entity.Position, new BoxCollider(8, 8));
                objectSystem.Create(trigger);
                _levelObjects.Add(trigger);
            }
            else if (entity.Name == "jump_platform")
            {
                JumpPadTrigger trigger = new JumpPadTrigger(entity.Position, new BoxCollider(8, 8));
                objectSystem.Create(trigger);
                _levelObjects.Add(trigger);
            }
            else if (entity.Name == "dash_crystal")
            {
                DashCrystalTrigger trigger = new DashCrystalTrigger(entity.Position, new BoxCollider(8, 8));
                objectSystem.Create(trigger);
                _levelObjects.Add(trigger);
            }
            else if (entity.Name == "fruit")
            {
                FruitTrigger trigger = new FruitTrigger(entity.Position, new BoxCollider(8, 8));
                objectSystem.Create(trigger);
                _levelObjects.Add(trigger);
            }
        }

        return _levels[_currentLevel];
    }

}