using System;
using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public class Decal : IGameObject, IDrawable
    {
        protected ObjectSystem _objectSystem;
        protected CoreEngine _coreEngine;
        protected Vector2 _position;

        public Vector2 Position => _position;
        
        protected IRenderer _renderer;
        public IRenderer Renderer => _renderer;
        public int DrawOrder => _renderer.DrawOrder;

        public Decal(Vector2 position)
        {
            _coreEngine = DI.Get<CoreEngine>();
            _objectSystem = DI.Get<ObjectSystem>();
            _position = position;

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
        
    }
}