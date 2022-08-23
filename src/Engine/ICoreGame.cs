namespace Engine
{

    public struct WindowData
    {
        public int Width;
        public int Height;
        public int Zoom;
        public string WindowTitle;
    }
    
    public interface ICoreGame
    {

        float Speed { get; }

        WindowData GetWindowData();
        
        void Initialize();
        void LoadAssets(AssetLoader assetLoader);
    }

}