using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class HomingMissileMotionComponent : Node
{
    [Export] public LaunchComponent Launch { get; set; }
    [Export] public float MaxSpeed { get; set; } = 16f;
    [Export] public float Acceleration { get; set; } = 8f;
    [Export] public float MaxTurnRate { get; set; } = 180f; // degrees per second
    [Export] public float WobbleStrength { get; set; } = 5f; // degrees
    [Export] public float Drag { get; set; } = 0.98f;
    [Export] public float SteeringLerp { get; set; } = 0.05f; // low = sluggish, high = responsive
    [Export] public Area2D DetectionArea { get; set; }
    
    private Vector2 _steeringDirection = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;
    private Node2D _target = null;

    public override void _Ready()
    {
        DetectionArea.BodyEntered += OnBodyEntered;
        _velocity = Launch.GetInitialVelocity();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Launch == null) return;
        if (Owner is not Node2D owner) return;
        if (_target == null)
        {
            owner.Position += _velocity * (float)delta;
            return;
        }
        
        var toTarget = (_target.GlobalPosition - owner.GlobalPosition).Normalized();
        _steeringDirection = _steeringDirection.Lerp(toTarget, SteeringLerp);
        
        var angleToTarget = _velocity.AngleTo(_steeringDirection);
        var maxAngle = Mathf.DegToRad(MaxTurnRate) * (float)delta;
        var clampedAngle = Mathf.Clamp(angleToTarget, -maxAngle, maxAngle);

        var rng = new RandomNumberGenerator();
        var wobble = Mathf.DegToRad(rng.RandfRange(-WobbleStrength, WobbleStrength));
        clampedAngle += wobble;
        
        _velocity = _velocity.Rotated(clampedAngle);
        _velocity *= Drag;

        var desiredSpeed = Mathf.Min(MaxSpeed, _velocity.Length() + Acceleration * (float)delta);
        _velocity = _velocity.Normalized() * desiredSpeed;
        
        owner.Position += _velocity * (float)delta;
        owner.Rotation = _velocity.Angle();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (_target != null) return;
        if (body == null) return;
        
        _target = body;
    }
}