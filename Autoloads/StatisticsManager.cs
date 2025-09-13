using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.Autoloads;

public partial class StatisticsManager : Node
{
    private GameManager _gameManager;
    private AchievementManager _achievementManager;
    private Dictionary<string, Variant> _stats = new();

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _achievementManager = GetNode<AchievementManager>("/root/AchievementManager");
        LoadStatistics();
    }

    private void LoadStatistics()
    {
        if (_gameManager.PlayerState.TryGetValue("statistics", out var statsObj))
        {
            _stats = (Dictionary<string, Variant>)statsObj;
        }
        else
        {
            _stats = new Dictionary<string, Variant>();
            _gameManager.PlayerState["statistics"] = _stats;
        }
    }
    
    /// <summary>
    /// Increases a numerical statistic by a given amount.
    /// </summary>
    public void IncrementStat(string statName, int amount = 1)
    {
        if (_stats.TryGetValue(statName, out var currentValue))
        {
            _stats[statName] = (int)currentValue + amount;
        }
        else
        {
            _stats[statName] = amount;
        }
        GD.Print($"Stat '{statName}' updated to: {_stats[statName]}");
        CheckAchievementsForStat(statName);
    }

    /// <summary>
    /// Gets the value of a statistic.
    /// </summary>
    public Variant GetStat(string statName, Variant defaultValue = default)
    {
        return _stats.TryGetValue(statName, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Checks if the updated stat meets the criteria for any achievements.
    /// </summary>
    private void CheckAchievementsForStat(string statName)
    {
        switch (statName)
        {
            case "enemies_defeated":
                if ((int)GetStat(statName, 0) >= 100)
                {
                    _achievementManager.UnlockAchievement("slayer_100_enemies");
                }
                break;
            case "jumps_made":
                if ((int)GetStat(statName, 0) >= 1000)
                {
                    _achievementManager.UnlockAchievement("super_jumper");
                }
                break;
        }
    }
}