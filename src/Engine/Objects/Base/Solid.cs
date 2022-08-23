using System;
using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public class Solid : IGameObject, IDrawable, IPhysicsObject
    {
        protected CoreEngine _coreEngine;
        protected ObjectSystem _objectSystem;
        protected Vector2 _position;
        protected BoxCollider _collider;

        public Vector2 Position => _position;
        public BoxCollider Collider => _collider;
        
        protected IRenderer _renderer;
        public int DrawOrder => _renderer.DrawOrder;

        public Solid(Vector2 position, BoxCollider collider)
        {
            _coreEngine = DI.Get<CoreEngine>();
            _objectSystem = DI.Get<ObjectSystem>();
            _position = position;
            _collider = collider;

            _renderer = null;
        }

        public virtual void OnDestroy()
        {
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
            _renderer.Draw(_position);
        }

        public virtual bool DealDamage(float damage)
        {
            return false;
        }
    }
}