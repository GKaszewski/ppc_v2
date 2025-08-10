using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class LaunchComponent : Node2D
{
    [Export] public Vector2 InitialDirection { get; set; } = Vector2.Right;
    [Export] public float Speed { get; set; } = 16f;
    [Export] public Vector2 SpawnPosition { get; set; } = Vector2.Zero;
    [Export] public float SpawnRotation { get; set; } = 0f;

    public override void _Ready()
    {
        if (Owner is not Node2D root) return;
        
        root.GlobalPosition = SpawnPosition;
        root.GlobalRotation = SpawnRotation;
    }
    
    public Vector2 GetInitialVelocity()
    {
        return InitialDirection.Normalized() * Speed;
    }
}