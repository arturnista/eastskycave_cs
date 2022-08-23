using Raylib_cs;

namespace Engine.Input
{

    public class InputSystem
    {
        public KeyboardKey[] Right { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_D, KeyboardKey.KEY_RIGHT };
        public KeyboardKey[] Left { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_A, KeyboardKey.KEY_LEFT };
        public KeyboardKey[] Up { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_W, KeyboardKey.KEY_UP };
        public KeyboardKey[] Down { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_S, KeyboardKey.KEY_DOWN };
        public KeyboardKey[] Jump { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_L, KeyboardKey.KEY_SPACE, KeyboardKey.KEY_C };
        public KeyboardKey[] Dash { get; protected set; } = new KeyboardKey[] { KeyboardKey.KEY_K, KeyboardKey.KEY_X };

        public bool IsKeyDown(KeyboardKey[] keys)
        {
            foreach (var key in keys)
            {
                if (Raylib.IsKeyDown(key)) return true;
            }
            return false;
        }
        
        public bool IsKeyUp(KeyboardKey[] keys)
        {
            foreach (var key in keys)
            {
                if (Raylib.IsKeyUp(key)) return true;
            }
            return false;
        }

        public bool IsKeyPressed(KeyboardKey[] keys)
        {
            foreach (var key in keys)
            {
                if (Raylib.IsKeyPressed(key)) return true;
            }
            return false;
        }

        public bool IsKeyReleased(KeyboardKey[] keys)
        {
            foreach (var key in keys)
            {
                if (Raylib.IsKeyReleased(key)) return true;
            }
            return false;
        }

    }
    
}