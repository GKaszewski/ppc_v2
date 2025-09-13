using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

[GlobalClass]
public partial class SpeedRunEventHandler : Node
{
    private SpeedRunManager _speedRunManager;

    public override void _Ready()
    {
        _speedRunManager = GetNode<SpeedRunManager>("/root/SpeedRunManager");
        var eventBus = GetNode<EventBus>("/root/EventBus");
        
        eventBus.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(int levelIndex, Node currentScene, double completionTime)
    {
        _speedRunManager.Split();
    }
}