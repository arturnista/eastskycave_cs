using System.Numerics;
using Engine;
using Engine.Objects;
using Newtonsoft.Json;

public class CoreGame : ICoreGame
{
    
    private const string SettingsPath = "./resources/settings/";
    private PlayerActor _player;
    private IMap _map;
    private GameSettings _game;
    private AssetsSettings _assets;

    public float Speed { get; protected set; }

    public CoreGame()
    {
        string gameSettingsPath = Path.Combine(SettingsPath, "game.json");
        string gameSettingsJson = System.IO.File.ReadAllText(gameSettingsPath);
        _game = JsonConvert.DeserializeObject<GameSettings>(gameSettingsJson);

        string assetsSettingsPath = Path.Combine(SettingsPath, "assets.json");
        string assetSettingsJson = System.IO.File.ReadAllText(assetsSettingsPath);
        _assets = JsonConvert.DeserializeObject<AssetsSettings>(assetSettingsJson);

        GameStaticData.WindowHeight = _game.general.screenHeight;
        GameStaticData.WindowWidth = _game.general.screenWidth;
        GameStaticData.Zoom = _game.general.zoom;

        DI.Set(this);
    }

    public WindowData GetWindowData()
    {
        return new WindowData()
        {
            Width = _game.general.screenWidth,
            Height = _game.general.screenHeight,
            Zoom = _game.general.zoom,
            WindowTitle = _game.general.name
        };
    }

    public void LoadAssets(AssetLoader assetLoader)
    {
        foreach (var item in _assets.images)
        {
            if (item.Value.isSheet)
            {
                assetLoader.LoadSpritesheet(item.Key, item.Value.src, item.Value.width, item.Value.height);
            }
            else
            {
                assetLoader.LoadSprite(item.Key, item.Value.src);
            }
        }

        _map = new OgmoMap();
        DI.Set<IMap>(_map);
        foreach (var item in _assets.levels)
        {
            _map.LoadLevel(item.Key, item.Value);
        }
    }

    public void Initialize()
    {
        var level = _map.ChangeLevel(_game.gameplay.initialLevel);
        Vector2 spawnPosition = Vector2.Zero;
        foreach (var item in level.Entities) if (item.Name == "player_spawn") spawnPosition = item.Position;

        _player = new PlayerActor(spawnPosition);
        DI.Get<ObjectSystem>().Create(_player);
        DI.Set<PlayerActor>(_player);

        Camera camera = DI.Get<Camera>();
        camera.SetFollower(_player);
        camera.SetMap(_map);

        new DrawDebug();
    }

    public void PlayerDeath()
    {
        _player.Death();

        _map.Reset();
        
        var level = _map.CurrentLevel;
        foreach (var entity in level.Entities)
        {
            if (entity.Name == "player_spawn")
            {
                _player.MoveTo(entity.Position);
            }
        }
    }
}