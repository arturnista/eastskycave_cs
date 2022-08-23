using System.Numerics;
using Engine.Objects;
using Engine.Objects.ParticleSystem;

public class DashCrystalTrigger : Trigger
{

    private float _time;
    private bool _isEnabled;
    private SheetRenderer _sheetRenderer;

    private ParticleEmitter? _effectParticles;

    public DashCrystalTrigger(Vector2 position, BoxCollider collider) : base(position, collider)
    {
        _sheetRenderer = new SheetRenderer("items", 2);
        SetRenderer(_sheetRenderer);
        _isEnabled = true;

        _effectParticles = new ParticleEmitter(_position, ParticleEmitterSettings.DashCrystalEffect());
        _objectSystem.Create(_effectParticles);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _objectSystem.Destroy(_effectParticles);
        _effectParticles = null;
        _coreEngine.OnFrame -= OnFrame;
    }

    public override void OnTrigger(Actor other)
    {
        if (other is PlayerActor player)
        {
            if (!_isEnabled) return;

            player.AllowDash();
            _effectParticles.Stop();
            
            _isEnabled = false;
            _sheetRenderer.SetIndex(3);
            _time = 0f;
            _coreEngine.OnFrame += OnFrame;
        }
    }

    private void OnFrame(float deltatime)
    {
        _time += deltatime;
        if (_time > 5f)
        {
            _effectParticles.Start();
            _isEnabled = true;
            _time = -1f;
            _sheetRenderer.SetIndex(2);
            _coreEngine.OnFrame -= OnFrame;
        }
    }
}