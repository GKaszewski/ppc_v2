using Godot;

namespace Mr.BrickAdventures.Autoloads;

public partial class EventBus : Node
{
    [Signal] public delegate void LevelStartedEventHandler(int levelIndex, Node currentScene);
    [Signal] public delegate void LevelCompletedEventHandler(int levelIndex, Node currentScene, double completionTime);
}