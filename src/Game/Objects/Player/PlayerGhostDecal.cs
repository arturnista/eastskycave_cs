using System;
using System.Numerics;
using Engine.Objects;
using Raylib_cs;

public class PlayerGhostDecal : Decal
{
    private float _lifeTime;
    private float _time;

    public PlayerGhostDecal(Vector2 position, float lifeTime) : base(position)
    {
        _lifeTime = lifeTime;
        _coreEngine.OnFrame += OnFrame;

        SetRenderer(new SheetRenderer("player", 2));
        _renderer.Color = new Color(0, 150, 255, 100);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _coreEngine.OnFrame -= OnFrame;
    }

    private void OnFrame(float deltatime)
    {
        _time += deltatime;
        if (_time > _lifeTime)
        {
            _objectSystem.Destroy(this);
        }
    }
}