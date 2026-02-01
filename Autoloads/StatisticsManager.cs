using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Manages game statistics using GameStateStore.
/// </summary>
public partial class StatisticsManager : Node
{

    /// <summary>
    /// Gets the statistics dictionary from the store.
    /// </summary>
    private Dictionary<string, int> GetStats()
    {
        return GameStateStore.Instance?.Player.Statistics ?? new Dictionary<string, int>();
    }

    /// <summary>
    /// Increases a numerical statistic by a given amount.
    /// </summary>
    public void IncrementStat(string statName, int amount = 1)
    {
        var stats = GetStats();
        if (stats.TryGetValue(statName, out var currentValue))
        {
            stats[statName] = currentValue + amount;
        }
        else
        {
            stats[statName] = amount;
        }
    }

    /// <summary>
    /// Sets a statistic to a specific value.
    /// </summary>
    public void SetStat(string statName, int value)
    {
        var stats = GetStats();
        stats[statName] = value;
    }

    /// <summary>
    /// Gets the value of a statistic.
    /// </summary>
    public int GetStat(string statName)
    {
        var stats = GetStats();
        return stats.TryGetValue(statName, out var value) ? value : 0;
    }

    /// <summary>
    /// Gets a copy of all statistics.
    /// </summary>
    public Dictionary<string, int> GetAllStats()
    {
        return new Dictionary<string, int>(GetStats());
    }
}