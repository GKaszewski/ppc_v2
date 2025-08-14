using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class SpaceshipEnterComponent : Node
{
    [Export] public Area2D Area { get; set; }
    [Signal] public delegate void SpaceshipEnteredEventHandler();

    public override void _Ready()
    {
        Area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not PlayerController) return;
        EmitSignalSpaceshipEntered();
        Owner.QueueFree();
    }
}