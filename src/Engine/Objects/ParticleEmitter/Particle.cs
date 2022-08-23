using System.Numerics;
using Raylib_cs;

namespace Engine.Objects.ParticleSystem
{
    
    public class Particle : IDrawable
    {
        public delegate void DisableHandler();
        public event DisableHandler OnDisable;

        private ParticleEmitter _particleSystem;
        private CoreEngine _coreEngine;
        private ParticleEmitterSettings _settings;

        private Color _color;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _lifeTime;

        public int DrawOrder => DrawableLayers.ParticleLayer;

        public Particle(ParticleEmitter particleSystem)
        {
            _particleSystem = particleSystem;
            _coreEngine = DI.Get<CoreEngine>();
        }

        public void Enable(Vector2 position, ParticleEmitterSettings settings)
        {
            _position = position;
            _settings = settings;
            _lifeTime = _settings.ParticleLifeTime;
            _color = _settings.Color;
            _velocity = Vector2.Zero;
            _velocity.X = MathHelper.RandomRange(_settings.MinInitialVelocity.X, _settings.MaxInitialVelocity.X);
            _velocity.Y = MathHelper.RandomRange(_settings.MinInitialVelocity.Y, _settings.MaxInitialVelocity.Y);
            _coreEngine.OnFrame += Frame;
            _coreEngine.RegisterDraw(this);
        }

        private void Frame(float deltatime)
        {
            _position += _velocity * deltatime;
            _velocity -= _settings.Drag * deltatime;

            _lifeTime -= deltatime;
            if (_lifeTime < 0)
            {
                Disable();
                return;
            }

            if (_settings.HasColorLerp)
            {
                _color = MathHelper.ColorLerp(_settings.Color, _settings.FinalColor, 1f - (_lifeTime / _settings.ParticleLifeTime));
            }
        }

        public void Draw()
        {
            var rec = _position;
            rec.Y = -rec.Y;
            Raylib.DrawRectangleV(rec, Vector2.One, _color);
        }

        private void Disable()
        {
            OnDisable?.Invoke();
            Destroy();
        }

        public void Destroy()
        {
            _coreEngine.OnFrame -= Frame;
            _coreEngine.RemoveDrawer(this);
        }
    }

}
