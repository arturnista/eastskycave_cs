using System.Numerics;
using Raylib_cs;

namespace Engine.Objects.ParticleSystem
{

    public struct ParticleEmitterSettings
    {
        public string Name = "";
        public bool AutoStart = true;
        public float SpawnRate = -1f;
        public float LifeTime = -1f;
        public float ParticleLifeTime = 2f;
        public float SpawnRange = 0f;
        public int StartAmount = 0;
        public Vector2 MinInitialVelocity = Vector2.Zero;
        public Vector2 MaxInitialVelocity = Vector2.Zero;
        public Vector2 Drag = Vector2.Zero;
        public Color Color = Color.WHITE;
        public Color FinalColor = Color.WHITE;
        public bool HasColorLerp = false;

        public ParticleEmitterSettings()
        {
        }

        public static ParticleEmitterSettings LandEffect()
        {
            return new ParticleEmitterSettings()
            {
                Name = "LandEffect",
                LifeTime = .4f,
                ParticleLifeTime = .4f,
                SpawnRange = GameStaticData.TileSize / 2,
                StartAmount = 7,
                MinInitialVelocity = new Vector2(-10f, 30f),
                MaxInitialVelocity = new Vector2(10f, 50f),
                Drag = Vector2.UnitY * GameStaticData.Gravity,
                HasColorLerp = true,
                Color = new Color(121, 65, 0, 255),
                FinalColor = new Color(121, 65, 0, 0)
            };
        }

        public static ParticleEmitterSettings DashEffect(Vector2 dashVelocity, Vector2 dashDrag)
        {
            return new ParticleEmitterSettings()
            {
                Name = "DashEffect",
                LifeTime = 2f,
                ParticleLifeTime = MathHelper.RandomRange(0.7f, 2f),
                SpawnRange = GameStaticData.TileSize / 2,
                StartAmount = 8,
                MinInitialVelocity = dashVelocity * 0.1f,
                MaxInitialVelocity = dashVelocity * 0.2f,
                // Drag = dashDrag * 0.5f,
                HasColorLerp = true,
                Color = new Color(0, 150, 255, 255),
                FinalColor = new Color(0, 150, 255, 0)
            };
        }

        public static ParticleEmitterSettings DashCrystalEffect()
        {
            return new ParticleEmitterSettings()
            {
                Name = "DashCrystalEffect",
                LifeTime = -1,
                ParticleLifeTime = 2f,
                SpawnRate = .2f,
                MinInitialVelocity = Vector2.One * -GameStaticData.TileSize,
                MaxInitialVelocity = Vector2.One * GameStaticData.TileSize,
                HasColorLerp = true,
                Color = new Color(255, 243, 146, 255),
                FinalColor = new Color(255, 243, 146, 0)
            };
        }

        public static ParticleEmitterSettings CollectFruitEffect()
        {
            return new ParticleEmitterSettings()
            {
                Name = "CollectFruitEffect",
                LifeTime = 2f,
                ParticleLifeTime = 2f,
                SpawnRange = GameStaticData.TileSize / 2,
                StartAmount = 25,
                MinInitialVelocity = Vector2.One * -GameStaticData.TileSize,
                MaxInitialVelocity = Vector2.One * GameStaticData.TileSize,
                HasColorLerp = true,
                Color = new Color(0, 150, 255, 255),
                FinalColor = new Color(0, 150, 255, 0)
            };
        }
    }

}