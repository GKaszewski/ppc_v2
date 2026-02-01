using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

[GlobalClass]
public partial class GhostEventHandler : Node
{
    private GhostManager _ghostManager;

    public override void _Ready()
    {
        _ghostManager = GetNode<GhostManager>(Constants.GhostManagerPath);

        EventBus.Instance.LevelStarted += OnLevelStarted;
        EventBus.Instance.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelStarted(int levelIndex, Node currentScene)
    {
        GD.Print($"GhostEventHandler: Level {levelIndex} started.");
        _ghostManager.StartRecording(levelIndex);
        _ghostManager.SpawnGhostPlayer(levelIndex, currentScene);
    }

    private void OnLevelCompleted(int levelIndex, Node currentScene, double completionTime)
    {
        _ghostManager.StopRecording(true, completionTime);
    }
}