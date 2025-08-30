using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.Autoloads;

public partial class AchievementManager : Node
{
    [Export] private string AchievementsFolderPath = "res://achievements/";
    [Export] private PackedScene AchievementPopupScene { get; set; }
    
    private System.Collections.Generic.Dictionary<string, AchievementResource> _achievements = new();
    private Array<string> _unlockedAchievementIds = [];
    private GameManager _gameManager;
    
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        LoadAchievementsFromFolder();
        LoadUnlockedAchievements();
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
    
    public void UnlockAchievement(string achievementId)
    {
        if (!_achievements.TryGetValue(achievementId, out var achievement))
        {
            GD.PrintErr($"Attempted to unlock non-existent achievement: '{achievementId}'");
            return;
        }

        if (_unlockedAchievementIds.Contains(achievementId))
        {
            return; // Already unlocked
        }

        // 1. Mark as unlocked internally
        _unlockedAchievementIds.Add(achievementId);
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
        
        // 4. Save progress
        SaveUnlockedAchievements();
    }
    
    public void LockAchievement(string achievementId)
    {
        if (_unlockedAchievementIds.Contains(achievementId))
        {
            _unlockedAchievementIds.Remove(achievementId);
            SaveUnlockedAchievements();
        }
    }
    
    private void SaveUnlockedAchievements()
    {
        _gameManager.PlayerState["unlocked_achievements"] = _unlockedAchievementIds;
        // You might want to trigger a save game here, depending on your SaveSystem
    }

    private void LoadUnlockedAchievements()
    {
        if (_gameManager.PlayerState.TryGetValue("unlocked_achievements", out var unlocked))
        {
            _unlockedAchievementIds = (Array<string>)unlocked;
        }
    }
}