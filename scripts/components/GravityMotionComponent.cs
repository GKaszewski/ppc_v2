using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class GravityMotionComponent : Node2D
{
    [Export] public CharacterBody2D Body { get; set; }
    [Export] public LaunchComponent LaunchComponent { get; set; }
    [Export] public Vector2 Gravity { get; set; } = new Vector2(0, 1000f);
    [Export] public Vector2 TargetDirection { get; set; } = Vector2.Up;

    private Vector2 _velocity = Vector2.Zero;

    public override void _Ready()
    {
        if (LaunchComponent == null) return;
        
        var direction = LaunchComponent.InitialDirection.X > 0f ? TargetDirection : new  Vector2(-TargetDirection.X, TargetDirection.Y);
        direction = direction.Normalized();
        _velocity = direction * LaunchComponent.Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Body == null) return;
        
        _velocity += Gravity * (float)delta;
        Body.Velocity = _velocity;

        Body.MoveAndSlide();

        if (_velocity.LengthSquared() > 0.01f)
        {
            Body.Rotation = _velocity.Angle();
        }
    }
}