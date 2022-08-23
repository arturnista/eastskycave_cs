using System.Numerics;
using Engine.Objects;

public class DeathTrigger : Trigger
{

    public DeathTrigger(Vector2 position, BoxCollider collider) : base(position, collider)
    {
    }

    public override void OnTrigger(Actor other)
    {
        if (other is PlayerActor player)
        {
            DI.Get<CoreGame>().PlayerDeath();
        }
    }

}