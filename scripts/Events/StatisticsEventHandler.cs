using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.Events;

/// <summary>
/// Handles game events and updates statistics accordingly.
/// Listens to EventBus signals and increments relevant stats.
/// </summary>
public partial class StatisticsEventHandler : Node
{
    private StatisticsManager _statisticsManager;

    public override void _Ready()
    {
        _statisticsManager = GetNode<StatisticsManager>(Constants.StatisticsManagerPath);

        // Subscribe to events
        EventBus.Instance.CoinCollected += OnCoinCollected;
        EventBus.Instance.EnemyDefeated += OnEnemyDefeated;
        EventBus.Instance.PlayerDied += OnPlayerDied;
        EventBus.Instance.LevelCompleted += OnLevelCompleted;
        EventBus.Instance.ChildRescued += OnChildRescued;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance == null) return;

        EventBus.Instance.CoinCollected -= OnCoinCollected;
        EventBus.Instance.EnemyDefeated -= OnEnemyDefeated;
        EventBus.Instance.PlayerDied -= OnPlayerDied;
        EventBus.Instance.LevelCompleted -= OnLevelCompleted;
        EventBus.Instance.ChildRescued -= OnChildRescued;
    }

    private void OnCoinCollected(int amount, Vector2 position)
    {
        _statisticsManager.IncrementStat("coins_collected", amount);
    }

    private void OnEnemyDefeated(Node enemy, Vector2 position)
    {
        _statisticsManager.IncrementStat("enemies_defeated");
    }

    private void OnPlayerDied(Vector2 position)
    {
        _statisticsManager.IncrementStat("deaths");
    }

    private void OnLevelCompleted(int levelIndex, Node currentScene, double completionTime)
    {
        _statisticsManager.IncrementStat("levels_completed");
    }

    private void OnChildRescued(Vector2 position)
    {
        _statisticsManager.IncrementStat("children_rescued");
    }
}
