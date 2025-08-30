using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class ExitDoorComponent : Area2D, IUnlockable
{
    [Export] public bool Locked { get; set; } = true;
    [Export] public Sprite2D DoorSprite { get; set; }
    [Export] public AudioStreamPlayer2D OpenDoorSfx { get; set; }
    [Export] public int OpenedDoorFrame { get; set; } = 0;
    [Export] public string AchievementId = "level_complete_1";
    
    [Signal] public delegate void ExitTriggeredEventHandler();
    
    private GameManager _gameManager;
    private AchievementManager _achievementManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _achievementManager = GetNode<AchievementManager>("/root/AchievementManager");
        
        BodyEntered += OnExitAreaBodyEntered;
        
    }

    private void OnExitAreaBodyEntered(Node2D body)
    {
        if (Locked) return;
        
        EmitSignalExitTriggered();
        _achievementManager.UnlockAchievement(AchievementId);
        _gameManager.UnlockLevel((int)_gameManager.PlayerState["current_level"] + 1);
        CallDeferred(nameof(GoToNextLevel));
    }

    public void Unlock()
    {
        Locked = false;
        if (DoorSprite != null)
        {
            DoorSprite.Frame = OpenedDoorFrame;
        }

        OpenDoorSfx?.Play();
    }

    private void GoToNextLevel()
    {
        _gameManager.OnLevelComplete();
    }
}