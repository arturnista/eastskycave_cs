using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public interface IGameObject
    {
        Vector2 Position { get; }
        void OnDestroy();
    }
    
}