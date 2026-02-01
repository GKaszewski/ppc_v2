using System.Collections.Generic;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.State;

/// <summary>
/// Data for the current gameplay session.
/// Reset when player dies or completes a level.
/// </summary>
public class SessionState
{
    /// <summary>
    /// Current level index being played.
    /// </summary>
    public int CurrentLevel { get; set; }

    /// <summary>
    /// Coins collected during this session (not yet saved).
    /// </summary>
    public int CoinsCollected { get; set; }

    /// <summary>
    /// Skills unlocked during this session (not yet saved).
    /// </summary>
    public List<SkillData> SkillsUnlocked { get; set; } = new();

    /// <summary>
    /// Creates a fresh session state.
    /// </summary>
    public static SessionState CreateDefault() => new()
    {
        CurrentLevel = 0,
        CoinsCollected = 0,
        SkillsUnlocked = new List<SkillData>()
    };

    /// <summary>
    /// Resets session state to defaults.
    /// </summary>
    public void Reset()
    {
        CoinsCollected = 0;
        SkillsUnlocked.Clear();
    }

    /// <summary>
    /// Resets completely including level.
    /// </summary>
    public void ResetAll()
    {
        CurrentLevel = 0;
        CoinsCollected = 0;
        SkillsUnlocked.Clear();
    }
}
