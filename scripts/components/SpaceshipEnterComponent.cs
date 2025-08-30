using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class SpaceshipEnterComponent : Area2D
{
    [Signal] public delegate void SpaceshipEnteredEventHandler();

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not PlayerController player) return;
        player.SetSpaceshipMovement();
        EmitSignalSpaceshipEntered();
        QueueFree();
    }
}