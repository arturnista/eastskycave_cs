using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public interface IPhysicsObject
    {
        Vector2 Position { get; }
        BoxCollider Collider { get; }
    }
    
}