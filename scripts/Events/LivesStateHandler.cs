using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

/// <summary>
/// Handles player death events and updates lives in GameStateStore.
/// </summary>
public partial class LivesStateHandler : Node
{
    public override void _Ready()
    {
        EventBus.Instance.PlayerDied += OnPlayerDied;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.PlayerDied -= OnPlayerDied;
        }
    }

    private void OnPlayerDied(Vector2 position)
    {
        GameStateStore.Instance?.RemoveLife();
        GameStateStore.Instance?.ResetSession();
    }
}
