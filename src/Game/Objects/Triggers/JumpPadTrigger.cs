using System.Numerics;
using Engine.Objects;

public class JumpPadTrigger : Trigger
{

    private float _time;
    private bool _isAnimatingTile;
    private SheetRenderer _sheetRenderer;

    public JumpPadTrigger(Vector2 position, BoxCollider collider) : base(position, collider)
    {
        _sheetRenderer = new SheetRenderer("items", 0);
        SetRenderer(_sheetRenderer);
        _time = -1f;
    }

    public override void OnTrigger(Actor other)
    {
        if (other is PlayerActor player)
        {
            player.JumpPad();
            
            _sheetRenderer.SetIndex(1);

            if (!_isAnimatingTile) _coreEngine.OnFrame -= OnFrame;
            _isAnimatingTile = true;
            _time = 0f;
            _coreEngine.OnFrame += OnFrame;
        }
    }

    private void OnFrame(float deltatime)
    {
        _time += deltatime;
        if (_time > 0.5f)
        {
            _isAnimatingTile = false;
            _sheetRenderer.SetIndex(0);
            _coreEngine.OnFrame -= OnFrame;
        }
    }
}