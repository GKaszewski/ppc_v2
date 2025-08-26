using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class ExitDoorComponent : Node, IUnlockable
{
    [Export] public bool Locked { get; set; } = true;
    [Export] public Area2D ExitArea { get; set; }
    [Export] public Sprite2D DoorSprite { get; set; }
    [Export] public AudioStreamPlayer2D OpenDoorSfx { get; set; }
    [Export] public int OpenedDoorFrame { get; set; } = 0;
    
    [Signal] public delegate void ExitTriggeredEventHandler();
    
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        
        if (ExitArea == null)
        {
            GD.PushError("ExitDoorComponent: ExitArea is not set.");
            return;
        }
        
        ExitArea.BodyEntered += OnExitAreaBodyEntered;
        
    }

    private void OnExitAreaBodyEntered(Node2D body)
    {
        if (Locked) return;
        
        EmitSignalExitTriggered();
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