using System.IO;

public interface IMap
{
    OgmoLevel CurrentLevel { get; }
    void LoadLevel(string levelName, string levelPath);
    OgmoLevel ChangeLevel(string levelName);
    OgmoLevel Reset();
}