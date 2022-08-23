using System;
using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public abstract class Trigger : IGameObject, IDrawable, IPhysicsObject
    {

        protected CoreEngine _coreEngine;
        protected ObjectSystem _objectSystem;

        protected Vector2 _position;
        public Vector2 Position => _position;
        protected BoxCollider _collider;
        public BoxCollider Collider => _collider;

        protected IRenderer? _renderer;
        public int DrawOrder => _renderer.DrawOrder;

        public Trigger(Vector2 position, BoxCollider collider)
        {
            _position = position;
            _collider = collider;
            _renderer = null;
            _objectSystem = DI.Get<ObjectSystem>();
            _coreEngine = DI.Get<CoreEngine>();
            _coreEngine.OnPostFrame += OnPostFrame;
        }

        public virtual void OnDestroy()
        {
            _coreEngine.OnPostFrame -= OnPostFrame;
            if (_renderer != null)
            {
                _coreEngine.RemoveDrawer(this);
            }
        }

        public void SetRenderer(IRenderer renderer)
        {
            if (_renderer == null)
            {
                _coreEngine.RegisterDraw(this);
            }

            _renderer = renderer;
        }

        public virtual void Draw()
        {
            _renderer.Draw(Position);
        }

        private void OnPostFrame(float deltatime)
        {
            foreach (var actor in _objectSystem.Actors)
            {
                if (CollisionHelper.CollideWith(this, actor))
                {
                    OnTrigger(actor);
                    return;
                }
            }
        }

        public abstract void OnTrigger(Actor actor);

    }
    
}