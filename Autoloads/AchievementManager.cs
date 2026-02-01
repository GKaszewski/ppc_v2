using Godot;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Manages achievements using GameStateStore.
/// </summary>
public partial class AchievementManager : Node
{
    [Export] private string AchievementsFolderPath = "res://achievements/";
    [Export] private PackedScene AchievementPopupScene { get; set; }

    private System.Collections.Generic.Dictionary<string, AchievementResource> _achievements = new();

    public override void _Ready()
    {
        LoadAchievementsFromFolder();
    }

    private void LoadAchievementsFromFolder()
    {
        using var dir = DirAccess.Open(AchievementsFolderPath);
        if (dir == null)
        {
            GD.PrintErr($"AchievementManager: Could not open achievements folder at '{AchievementsFolderPath}'");
            return;
        }

        dir.ListDirBegin();
        var fileName = dir.GetNext();
        while (fileName != "")
        {
            if (!dir.CurrentIsDir() && fileName.EndsWith(".tres"))
            {
                var achievement = GD.Load<AchievementResource>(AchievementsFolderPath + fileName);
                if (achievement != null)
                {
                    _achievements.TryAdd(achievement.Id, achievement);
                }
            }
            fileName = dir.GetNext();
        }
    }

    /// <summary>
    /// Gets the list of unlocked achievement IDs from the store.
    /// </summary>
    private System.Collections.Generic.List<string> GetUnlockedIds()
    {
        return GameStateStore.Instance?.Player.UnlockedAchievements ?? new System.Collections.Generic.List<string>();
    }

    public void UnlockAchievement(string achievementId)
    {
        if (!_achievements.TryGetValue(achievementId, out var achievement))
        {
            GD.PrintErr($"Attempted to unlock non-existent achievement: '{achievementId}'");
            return;
        }

        var unlockedIds = GetUnlockedIds();
        if (unlockedIds.Contains(achievementId))
        {
            return; // Already unlocked
        }

        // 1. Mark as unlocked
        unlockedIds.Add(achievementId);
        GD.Print($"Achievement Unlocked: {achievement.DisplayName}");

        // 2. Show the UI popup
        if (AchievementPopupScene != null)
        {
            var popup = AchievementPopupScene.Instantiate<scripts.UI.AchievementPopup>();
            GetTree().Root.AddChild(popup);
            _ = popup.ShowAchievement(achievement);
        }

        // 3. Call SteamManager if it's available
        if (SteamManager.IsSteamInitialized)
        {
            SteamManager.UnlockAchievement(achievement.Id);
        }
    }

    public void LockAchievement(string achievementId)
    {
        var unlockedIds = GetUnlockedIds();
        if (unlockedIds.Contains(achievementId))
        {
            unlockedIds.Remove(achievementId);
        }
    }

    public bool IsAchievementUnlocked(string achievementId)
    {
        return GetUnlockedIds().Contains(achievementId);
    }
}