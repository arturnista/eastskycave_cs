using System;
using System.Numerics;
using Engine.Objects;
using ImGuiNET;
using Raylib_cs;

namespace Engine
{
    
    public class CoreEngine
    {

        public delegate void FrameHandler(float deltatime);

        public event FrameHandler? OnPreFrame;
        public event FrameHandler? OnFrame;
        public event FrameHandler? OnPostFrame;

        private List<IDrawable> _drawable = new List<IDrawable>();
        private List<IDrawableUI> _drawableUI = new List<IDrawableUI>();

        private ObjectSystem? _objectSystem;
        private AssetLoader? _assetLoader;
        private Camera? _camera;
        private PlayerActor? _player;
        private ICoreGame _game;
        private WindowData _windowData;

        public CoreEngine(ICoreGame game)
        {
            _game = game;
            DI.Set(this);
        }

        public void Initialize()
        {
            _windowData = _game.GetWindowData();
            Raylib.InitWindow(_windowData.Width, _windowData.Height, _windowData.WindowTitle);
            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            Vector2 offset = new Vector2(
                Raylib.GetScreenWidth() / 2f,
                Raylib.GetScreenHeight() / 2f
            );

            var camera2D = new Camera2D(offset, Vector2.Zero, 0f, _windowData.Zoom);
            _camera = new Camera(camera2D);
            DI.Set(_camera);

            _objectSystem = new ObjectSystem();
            DI.Set(_objectSystem);

            DI.Set(new Input.InputSystem());

            _assetLoader = new AssetLoader();
            DI.Set(_assetLoader);

            _game.LoadAssets(_assetLoader);
            _game.Initialize();

            Raylib.SetTargetFPS(60);

        }

        public void Loop()
        {
            
            var backgroundColor = new Color(121, 65, 0, 255);
            while (!Raylib.WindowShouldClose())
            {
                float deltatime = Raylib.GetFrameTime();

                _objectSystem.Update();

                // Update object system
                OnPreFrame?.Invoke(deltatime);
                OnFrame?.Invoke(deltatime);
                OnPostFrame?.Invoke(deltatime);

                // Draw
                Raylib.BeginDrawing();
                {
                    Raylib.ClearBackground(backgroundColor);

                    // World drawing
                    Raylib.BeginMode2D(_camera.Camera2D);
                    {
                        List<IDrawable> drawables = new List<IDrawable>(_drawable);
                        drawables.Sort((a, b) => a.DrawOrder - b.DrawOrder);
                        foreach (var item in drawables)
                        {
                            item.Draw();
                        }
                    }
                    Raylib.EndMode2D();

                    List<IDrawableUI> drawablesUI = new List<IDrawableUI>(_drawableUI);
                    drawablesUI.Sort((a, b) => a.UIDrawOrder - b.UIDrawOrder);
                    foreach (var item in drawablesUI)
                    {
                        item.UIDraw(_windowData.Zoom);
                    }
                }
                Raylib.EndDrawing();
            }
        }

        public void Close()
        {
            Raylib.CloseWindow();
        }

        public void RegisterDraw(IDrawable drawable)
        {
            _drawable.Add(drawable);
        }

        public void RemoveDrawer(IDrawable drawable)
        {
            _drawable.Remove(drawable);
        }

        public void RegisterDrawUI(IDrawableUI drawable)
        {
            _drawableUI.Add(drawable);
        }

        public void RemoveDrawerUI(IDrawableUI drawable)
        {
            _drawableUI.Remove(drawable);
        }

    }

}