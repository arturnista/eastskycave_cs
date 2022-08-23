using System.Numerics;
using Engine.Objects;
using Raylib_cs;

public static class CollisionHelper
{

    public static bool CollideWith(IPhysicsObject origin, IPhysicsObject moving)
    {
        return CollideWith(origin, moving, Vector2.Zero);
    }
    
    public static bool CollideWith(IPhysicsObject origin, IPhysicsObject moving, Vector2 motion)
    {
        var originBounds = origin.Collider.GetBounds(origin.Position);
        var movingBounds = moving.Collider.GetBounds(moving.Position + motion);

        if ((movingBounds.MinX >= originBounds.MinX && movingBounds.MinX <= originBounds.MaxX) || (movingBounds.MaxX >= originBounds.MinX && movingBounds.MaxX <= originBounds.MaxX))
        {
            if (movingBounds.MinY >= originBounds.MinY && movingBounds.MinY <= originBounds.MaxY)
            {
                return true;
            }

            if (movingBounds.MaxY >= originBounds.MinY && movingBounds.MaxY <= originBounds.MaxY)
            {
                return true;
            }
        }

        return false;
    }

}