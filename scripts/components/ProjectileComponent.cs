using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class ProjectileComponent : Node2D
{
    [Export] public float Speed { get; set; } = 16f;
    [Export] public float AngleDirection { get; set; }
    [Export] public Vector2 SpawnPosition { get; set; } = Vector2.Zero;
    [Export] public float SpawnRotation { get; set; } = 0f;
    [Export] public CharacterBody2D Body { get; set; }

    public override void _Ready()
    {
        GlobalPosition = SpawnPosition;
        GlobalRotation = SpawnRotation;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Body == null) return;
        
        Body.Velocity += new Vector2(0f, -Speed).Rotated(AngleDirection);
        Body.MoveAndSlide();
    }
}