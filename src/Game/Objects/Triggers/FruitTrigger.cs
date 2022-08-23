using System.Numerics;
using Engine.Objects;
using Engine.Objects.ParticleSystem;

public class FruitTrigger : Trigger
{

    public enum FruitState
    {
        Free,
        Following,
        Collected
    }
    private FruitState _currentState;

    private Vector2 _lastPosition;
    private IGameObject _followObject;
    
    private float _collectTime;

    public FruitTrigger(Vector2 position, BoxCollider collider) : base(position, collider)
    {
        SetRenderer(new SpriteRenderer("fruit"));
        _renderer.DrawOrder = DrawableLayers.DefaultLayer;
        _followObject = null;

        _currentState = FruitState.Free;
    }

    public override void OnTrigger(Actor other)
    {
        if (_currentState != FruitState.Free) return;

        if (other is PlayerActor player)
        {
            if (_followObject != null) return;
            _currentState = FruitState.Following;
            _followObject = player.GetFruit(this);
            _coreEngine.OnFrame += OnFrame;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _coreEngine.OnFrame -= OnFrame;
    }

    private void OnFrame(float deltatime)
    {
        if (_currentState == FruitState.Following)
        {
            var target = _followObject.Position + new Vector2(-2, GameStaticData.TileSize + 2);
            _lastPosition = _followObject.Position;
            _position = MathHelper.Vector2Lerp(_position, target, 10f * deltatime);
        }
        else if (_currentState == FruitState.Collected)
        {
            _collectTime -= deltatime;
            if (_collectTime <= 0f)
            {
                _objectSystem.Create(new ParticleEmitter(_position, ParticleEmitterSettings.CollectFruitEffect()));
                _objectSystem.Destroy(this);
            }
        }
    }

    public void Collect(int index)
    {
        _currentState = FruitState.Collected;
        _collectTime = index * 0.05f;
    }

}