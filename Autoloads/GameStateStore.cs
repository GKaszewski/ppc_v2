using Godot;
using System.Collections.Generic;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Central store for game state - single source of truth.
/// Use the static Instance property for easy access.
/// </summary>
public partial class GameStateStore : Node
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static GameStateStore Instance { get; private set; }

    /// <summary>
    /// Persistent player state (saved to disk).
    /// </summary>
    public PlayerState Player { get; set; } = new();

    /// <summary>
    /// Current session state (transient, reset on death/level complete).
    /// </summary>
    public SessionState Session { get; set; } = new();

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
            Instance = null;
    }

    #region Coin Operations

    /// <summary>
    /// Gets total coins (saved + session).
    /// </summary>
    public int GetTotalCoins() => Player.Coins + Session.CoinsCollected;

    /// <summary>
    /// Adds coins to the session (not saved until level complete).
    /// </summary>
    public void AddSessionCoins(int amount)
    {
        Session.CoinsCollected += amount;
        EventBus.EmitCoinsChanged(GetTotalCoins());
    }

    /// <summary>
    /// Commits session coins to player state.
    /// </summary>
    public void CommitSessionCoins()
    {
        Player.Coins += Session.CoinsCollected;
        Session.CoinsCollected = 0;
    }

    /// <summary>
    /// Removes coins, first from session then from saved.
    /// </summary>
    public void RemoveCoins(int amount)
    {
        if (amount <= Session.CoinsCollected)
        {
            Session.CoinsCollected -= amount;
        }
        else
        {
            var remaining = amount - Session.CoinsCollected;
            Session.CoinsCollected = 0;
            Player.Coins = Mathf.Max(0, Player.Coins - remaining);
        }
        EventBus.EmitCoinsChanged(GetTotalCoins());
    }

    #endregion

    #region Lives Operations

    /// <summary>
    /// Decrements lives by 1.
    /// </summary>
    public void RemoveLife()
    {
        Player.Lives = Mathf.Max(0, Player.Lives - 1);
        EventBus.EmitLivesChanged(Player.Lives);
    }

    /// <summary>
    /// Adds lives.
    /// </summary>
    public void AddLives(int amount)
    {
        Player.Lives += amount;
        EventBus.EmitLivesChanged(Player.Lives);
    }

    #endregion

    #region Level Operations

    /// <summary>
    /// Unlocks a level for access.
    /// </summary>
    public void UnlockLevel(int levelIndex)
    {
        if (!Player.UnlockedLevels.Contains(levelIndex))
            Player.UnlockedLevels.Add(levelIndex);
    }

    /// <summary>
    /// Marks a level as completed and unlocks the next.
    /// </summary>
    public void MarkLevelComplete(int levelIndex)
    {
        if (!Player.CompletedLevels.Contains(levelIndex))
            Player.CompletedLevels.Add(levelIndex);
        UnlockLevel(levelIndex + 1);
    }

    /// <summary>
    /// Checks if a level is unlocked.
    /// </summary>
    public bool IsLevelUnlocked(int levelIndex) => Player.UnlockedLevels.Contains(levelIndex);

    #endregion

    #region Skill Operations

    /// <summary>
    /// Checks if a skill is unlocked (saved or session).
    /// </summary>
    public bool IsSkillUnlocked(SkillData skill)
    {
        return Player.UnlockedSkills.Contains(skill) || Session.SkillsUnlocked.Contains(skill);
    }

    /// <summary>
    /// Unlocks a skill in the session.
    /// </summary>
    public void UnlockSkillInSession(SkillData skill)
    {
        if (!IsSkillUnlocked(skill))
            Session.SkillsUnlocked.Add(skill);
    }

    /// <summary>
    /// Commits session skills to player state.
    /// </summary>
    public void CommitSessionSkills()
    {
        foreach (var skill in Session.SkillsUnlocked)
        {
            if (!Player.UnlockedSkills.Contains(skill))
                Player.UnlockedSkills.Add(skill);
        }
        Session.SkillsUnlocked.Clear();
    }

    /// <summary>
    /// Gets all unlocked skills from player persistence and current session.
    /// </summary>
    public List<SkillData> GetAllUnlockedSkills()
    {
        var result = new List<SkillData>(Player.UnlockedSkills);
        foreach (var skill in Session.SkillsUnlocked)
        {
            if (!result.Contains(skill)) result.Add(skill);
        }
        return result;
    }

    #endregion

    #region Reset Operations

    /// <summary>
    /// Resets only the session state.
    /// </summary>
    public void ResetSession() => Session.Reset();

    /// <summary>
    /// Resets everything to defaults.
    /// </summary>
    public void ResetAll()
    {
        Player.Reset();
        Session.ResetAll();
    }

    #endregion
}
