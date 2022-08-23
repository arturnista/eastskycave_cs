using System.Numerics;
using Engine;
using Engine.Objects;
using Raylib_cs;

public class DrawDebug : IDrawable, IDrawableUI
{

    public int DrawOrder => 100;
    public int UIDrawOrder => 0;

    private CoreEngine _coreEngine;
    private ObjectSystem _objSystem;
    private bool _shouldDisplay;
    private Camera _camera;
    private Vector2 _mouseWorldPosition;

    public DrawDebug()
    {
        _coreEngine = DI.Get<CoreEngine>();
        _objSystem = DI.Get<ObjectSystem>();
        _coreEngine.OnFrame += OnFrame;
        _coreEngine.RegisterDraw(this);
        _coreEngine.RegisterDrawUI(this);

        _shouldDisplay = false;
        _camera = DI.Get<Camera>();
    }

    private void OnFrame(float deltatime)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1)) _shouldDisplay = !_shouldDisplay;
    }

    public void Draw()
    {
        if (!_shouldDisplay) return;
        var objectSystem = DI.Get<ObjectSystem>();
        foreach (var solid in objectSystem.Solids)
        {
            Color color = Color.PINK;
            Raylib.DrawRectangleLines(
                (int)(solid.Position.X - solid.Collider.Rec.width / 2),
                -(int)(solid.Position.Y + solid.Collider.Rec.height / 2),
                (int)solid.Collider.Rec.width,
                (int)solid.Collider.Rec.height,
                color
            );
        }
        foreach (var trigger in objectSystem.Triggers)
        {
            Color color = trigger is DeathTrigger ? Color.RED : Color.GREEN;
            Raylib.DrawRectangleLines(
                (int)(trigger.Position.X - trigger.Collider.Rec.width / 2),
                -(int)(trigger.Position.Y + trigger.Collider.Rec.height / 2),
                (int)trigger.Collider.Rec.width,
                (int)trigger.Collider.Rec.height,
                color
            );
            
            if (trigger is JumpPadTrigger)
            {
                var bounds = trigger.Collider.GetBounds(trigger.Position, true);
                Raylib.DrawRectangleV(trigger.Position, Vector2.One * 8, Color.BLACK);
                var ii = new Vector2(bounds.MinX, -bounds.MinY);
                Raylib.DrawRectangleV(ii, Vector2.One, Color.WHITE);
                var ai = new Vector2(bounds.MaxX, -bounds.MinY);
                Raylib.DrawRectangleV(ai, Vector2.One, Color.WHITE);
                var ia = new Vector2(bounds.MinX, -bounds.MaxY);
                Raylib.DrawRectangleV(ia, Vector2.One, Color.WHITE);
                var aa = new Vector2(bounds.MaxX, -bounds.MaxY);
                Raylib.DrawRectangleV(aa, Vector2.One, Color.WHITE);
                Console.WriteLine($"{ii}, {ai}, {ia}, {aa}");
            }
        }
        foreach (var actor in objectSystem.Actors)
        {
            Raylib.DrawRectangleLines(
                (int)(actor.Position.X - actor.Collider.Rec.width / 2),
                -(int)(actor.Position.Y + actor.Collider.Rec.height / 2),
                (int)actor.Collider.Rec.width,
                (int)actor.Collider.Rec.height,
                Color.BLUE
            );
        }

        var mousePosition = Raylib.GetMousePosition();
        _mouseWorldPosition = Raylib.GetScreenToWorld2D(mousePosition, _camera.Camera2D);
        _mouseWorldPosition = new Vector2((int)_mouseWorldPosition.X, (int)_mouseWorldPosition.Y);
        Raylib.DrawRectangleV(_mouseWorldPosition, Vector2.One, Color.MAGENTA);

    }

    public void UIDraw(int zoom)
    {
        if (!_shouldDisplay) return;
        Raylib.DrawLine(0, GameStaticData.WindowHeight / 2, GameStaticData.WindowWidth, GameStaticData.WindowHeight / 2, Color.BLUE);
        Raylib.DrawLine(GameStaticData.WindowWidth / 2, 0, GameStaticData.WindowWidth / 2, GameStaticData.WindowHeight, Color.BLUE);

        Raylib.DrawText($"Actors: {_objSystem.Actors.Count}", 0, 15, 7, Color.WHITE);
        Raylib.DrawText($"Solids: {_objSystem.Solids.Count}", 0, 25, 7, Color.WHITE);
        Raylib.DrawText($"GameObjects: {_objSystem.GameObjects.Count}", 0, 35, 7, Color.WHITE);
        Raylib.DrawText($"Mouse: {_mouseWorldPosition.X}, {_mouseWorldPosition.Y}", 0, 45, 7, Color.WHITE);
    }

}