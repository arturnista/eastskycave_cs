using System.Numerics;

namespace Engine.Objects.ParticleSystem
{

    public class ParticleEmitter : IGameObject
    {
        private CoreEngine _coreEngine;
        private float _lifeTime;
        private float _spawnTime;
        private List<Particle> _particlesAlive = new List<Particle>();
        private List<Particle> _particlesAvailable = new List<Particle>();

        private Vector2 _position;
        public Vector2 Position => _position;

        private ParticleEmitterSettings _settings;
        public ParticleEmitterSettings Settings => _settings;

        public ParticleEmitter(Vector2 position, ParticleEmitterSettings settings)
        {
            _position = position;
            _settings = settings;
            _coreEngine = DI.Get<CoreEngine>();

            if (settings.AutoStart)
            {
                Start();
            }
        }

        public void OnDestroy()
        {
            _coreEngine.OnPostFrame -= PostFrame;
            foreach (var item in _particlesAlive)
            {
                item.Destroy();
            }
            foreach (var item in _particlesAvailable)
            {
                item.Destroy();
            }
            
            _particlesAlive.Clear();
            _particlesAvailable.Clear();
        }

        public void Start()
        {
            _coreEngine.OnPostFrame += PostFrame;

            _lifeTime = 0f;
            _spawnTime = 0f;

            for (int i = 0; i < _settings.StartAmount; i++)
            {
                CreateParticle();
            }
        }

        public void Stop()
        {
            _coreEngine.OnPostFrame -= PostFrame;
        }

        private void PostFrame(float deltatime)
        {
            if (_settings.SpawnRate > 0)
            {
                _spawnTime += deltatime;
                if (_spawnTime > _settings.SpawnRate)
                {
                    _spawnTime = 0f;
                    CreateParticle();
                }
            }

            if (_settings.LifeTime > 0)
            {
                _lifeTime += deltatime;
                if (_lifeTime > _settings.LifeTime)
                {
                    DI.Get<ObjectSystem>().Destroy(this);
                }
            }
        }

        public Particle CreateParticle()
        {
            var offset = new Vector2(
                MathHelper.RandomRange(-_settings.SpawnRange, _settings.SpawnRange),
                MathHelper.RandomRange(-_settings.SpawnRange, _settings.SpawnRange)
            );
            Particle particle;

            if (_particlesAvailable.Count > 0)
            {
                particle = _particlesAvailable[0];
                _particlesAvailable.Remove(particle);
            }
            else
            {
                particle = new Particle(this);
                particle.OnDisable += () => {
                    _particlesAlive.Remove(particle);
                    _particlesAvailable.Add(particle);
                };
            }

            _particlesAlive.Add(particle);
            particle.Enable(_position + offset, _settings);
            return particle;
        }
    }
    
}