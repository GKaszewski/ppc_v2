using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

/// <summary>
/// Handles level completion events and updates GameStateStore.
/// </summary>
public partial class LevelStateHandler : Node
{
    public override void _Ready()
    {
        EventBus.Instance.LevelCompleted += OnLevelCompleted;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.LevelCompleted -= OnLevelCompleted;
        }
    }

    private void OnLevelCompleted(int levelIndex, Node currentScene, double completionTime)
    {
        // State mutations (commit coins/skills, reset session) are handled by GameManager.OnLevelComplete
        // before this event fires. This handler is reserved for future level-specific side-effects.
    }
}
