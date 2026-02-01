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
        var store = GameStateStore.Instance;
        if (store == null) return;

        // Mark level complete and unlock next
        store.MarkLevelComplete(levelIndex);

        // Commit session data to persistent state
        store.CommitSessionCoins();
        store.CommitSessionSkills();

        // Reset session for next level
        store.ResetSession();
    }
}
