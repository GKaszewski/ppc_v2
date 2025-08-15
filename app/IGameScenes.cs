using Godot;

namespace Mr.BrickAdventures.app;

public interface IGameScenes
{
    void Load(PackedScene scene);
    void Restart();
    void ReturnToMain(PackedScene mainMenu);
}