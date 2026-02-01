using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
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
        _gameManager = GameManager.Instance;
        _achievementManager = GetNode<AchievementManager>(Constants.AchievementManagerPath);

        BodyEntered += OnExitAreaBodyEntered;

    }

    private void OnExitAreaBodyEntered(Node2D body)
    {
        if (Locked) return;

        EmitSignalExitTriggered();
        _achievementManager.UnlockAchievement(AchievementId);
        // Get current level from GameStateStore
        var currentLevel = GameStateStore.Instance?.Session.CurrentLevel ?? 0;
        _gameManager.UnlockLevel(currentLevel + 1);
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