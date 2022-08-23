using Engine;

namespace Game
{
    internal class Program
    {

        static void Main(string[] args)
        {
            CoreGame game = new CoreGame();
            
            CoreEngine engine = new CoreEngine(game);
            engine.Initialize();
            engine.Loop();
            engine.Close();
        }
    }
}