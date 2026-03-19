using System.Collections.Generic;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.State;

/// <summary>
/// Persistent player data that survives across sessions.
/// This is a POCO (Plain Old C# Object) for predictable state management.
/// </summary>
public class PlayerState
{
    private const int DefaultLives = 3;

    /// <summary>
    /// Saved coins (not including current session).
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// Remaining lives.
    /// </summary>
    public int Lives { get; set; } = DefaultLives;

    /// <summary>
    /// Indices of completed levels.
    /// </summary>
    public List<int> CompletedLevels { get; set; } = new();

    /// <summary>
    /// Indices of levels the player can access.
    /// </summary>
    public List<int> UnlockedLevels { get; set; } = new() { 0 };

    /// <summary>
    /// Skills the player has permanently unlocked.
    /// </summary>
    public List<SkillData> UnlockedSkills { get; set; } = new();

    /// <summary>
    /// Statistics dictionary for tracking game stats.
    /// </summary>
    public Dictionary<string, int> Statistics { get; set; } = new();

    /// <summary>
    /// IDs of unlocked achievements.
    /// </summary>
    public List<string> UnlockedAchievements { get; set; } = new();

    /// <summary>
    /// Creates a fresh default player state (same as <c>new PlayerState()</c>).
    /// </summary>
    public static PlayerState CreateDefault() => new();

    /// <summary>
    /// Resets this state to default values.
    /// </summary>
    public void Reset()
    {
        Coins = 0;
        Lives = DefaultLives;
        CompletedLevels.Clear();
        UnlockedLevels.Clear();
        UnlockedLevels.Add(0);
        UnlockedSkills.Clear();
        Statistics.Clear();
    }
}
