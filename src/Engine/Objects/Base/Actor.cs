using System;
using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public abstract class Actor : IGameObject, IDrawable, IPhysicsObject
    {

        protected IRenderer _renderer;
        protected Vector2 _position;
        protected BoxCollider _collider;
        protected CoreEngine _coreEngine;

        public Vector2 Position => _position;
        public BoxCollider Collider => _collider;
        public int DrawOrder => _renderer.DrawOrder;

        protected float _xRemaining;
        protected float _yRemaining;

        protected bool _isGrounded;

        protected ObjectSystem _objectSystem;

        public Actor(Vector2 position, BoxCollider collider)
        {
            _objectSystem = DI.Get<ObjectSystem>();
            _coreEngine = DI.Get<CoreEngine>();

            _renderer = null;
            
            _position = position;
            _collider = collider;
        }
        
        public virtual void OnDestroy()
        {
            if (_renderer != null) _coreEngine.RemoveDrawer(this);
        }

        public void SetRenderer(IRenderer renderer)
        {
            _renderer = renderer;
            _coreEngine.RegisterDraw(this);
        }

        public virtual void Draw()
        {
            _renderer.Draw(_position);
        }
        
        public void MoveTo(Vector2 position)
        {
            _position = position;
        }

        public void MoveX(float amount, Action<Solid> onCollide)
        {
            _xRemaining += amount;
            int move = (int)Math.Round(_xRemaining, 0);

            if (move != 0)
            {
                _xRemaining -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    foreach (var solid in _objectSystem.Solids)
                    {
                        if (CollisionHelper.CollideWith(solid, this, new Vector2(sign, 0)))
                        {
                            //Hit a solid!
                            onCollide?.Invoke(solid);
                            return;
                        }
                    }
                    _position.X += sign;
                    move -= sign;
                }
            }
        }

        public void MoveY(float amount, Action<Solid> onCollide)
        {
            _yRemaining += amount;
            int move = (int)Math.Round(_yRemaining, 0);

            if (move != 0)
            {
                _isGrounded = false;
                _yRemaining -= move;
                int sign = Math.Sign(move);
                while (move != 0)
                {
                    foreach (var solid in _objectSystem.Solids)
                    {
                        if (CollisionHelper.CollideWith(solid, this, new Vector2(0, sign)))
                        {
                            //Hit a solid!
                            onCollide?.Invoke(solid);
                            if (sign < 0) _isGrounded = true;
                            return;
                        }
                    }
                    _position.Y += sign;
                    move -= sign;
                }
            }
        }
    }
    
}