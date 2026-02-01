using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

[GlobalClass]
public partial class SpeedRunEventHandler : Node
{
    private SpeedRunManager _speedRunManager;

    public override void _Ready()
    {
        _speedRunManager = GetNode<SpeedRunManager>(Constants.SpeedRunManagerPath);

        EventBus.Instance.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(int levelIndex, Node currentScene, double completionTime)
    {
        _speedRunManager.Split();
    }
}