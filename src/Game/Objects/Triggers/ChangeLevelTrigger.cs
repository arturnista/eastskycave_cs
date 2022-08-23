using System.Numerics;
using Engine.Objects;

public class ChangeLevelTrigger : Trigger
{

    private string _levelName;

    public ChangeLevelTrigger(string levelName, Vector2 position, BoxCollider collider) : base(position, collider)
    {
        _levelName = levelName;
    }

    public override void OnTrigger(Actor other)
    {
        var level = DI.Get<IMap>().ChangeLevel(_levelName);
        foreach (var entity in level.Entities)
        {
            if (entity.Name == "player_spawn")
            {
                DI.Get<PlayerActor>().MoveTo(entity.Position);
            }
        }
    }

}