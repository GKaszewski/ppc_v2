using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class SpaceshipExitComponent : Area2D
{
    [Signal] public delegate void SpaceshipExitEventHandler();
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not PlayerController player) return;
        EmitSignalSpaceshipExit();
        player.SetPlatformMovement();
    }
}