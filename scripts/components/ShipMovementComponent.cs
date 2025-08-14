using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class ShipMovementComponent : Node, IMovement
{
    [Export] public float MaxSpeed { get; set; } = 200f;
    [Export] public float Acceleration { get; set; } = 100f;
    [Export] public float Friction { get; set; } = 50f;
    [Export] public CharacterBody2D Body { get; set; }
    
    public string MovementType { get; } = "ship";
    public bool Enabled { get; set; }
    public Vector2 PreviousVelocity { get; set; }
    
    private Vector2 _velocity = Vector2.Zero;
    
    public Vector2 Velocity => _velocity;
    public Vector2 LastDirection => _velocity.Normalized();

    public override void _PhysicsProcess(double delta)
    {
        if (Body == null || !Enabled) return;

        var inputVector = new Vector2(
            Input.GetActionStrength("right") - Input.GetActionStrength("left"),
            Input.GetActionStrength("down") - Input.GetActionStrength("up")
        ).Normalized();

        _velocity = inputVector != Vector2.Zero ? _velocity.MoveToward(inputVector * MaxSpeed, Acceleration * (float)delta) : _velocity.MoveToward(Vector2.Zero, Friction * (float)delta);

        _velocity = _velocity.LimitLength(MaxSpeed);
        Body.Velocity = _velocity;
        PreviousVelocity = Body.Velocity;
        Body.MoveAndSlide();
    }
}