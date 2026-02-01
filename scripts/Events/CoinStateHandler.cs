using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

/// <summary>
/// Handles coin collection events and updates the GameStateStore.
/// Replaces the manual coin logic in GameManager.
/// </summary>
public partial class CoinStateHandler : Node
{
    public override void _Ready()
    {
        EventBus.Instance.CoinCollected += OnCoinCollected;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.CoinCollected -= OnCoinCollected;
        }
    }

    private void OnCoinCollected(int amount, Vector2 position)
    {
        GameStateStore.Instance?.AddSessionCoins(amount);
    }
}
