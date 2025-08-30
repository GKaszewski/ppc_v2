using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GroundMovementAbility : MovementAbility
{
    [Export] public float MaxSpeed { get; set; } = 300.0f;
    [Export] public float Acceleration { get; set; } = 2000.0f;
    [Export] public float Friction { get; set; } = 1500.0f;
    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (_input == null) return Vector2.Zero;
        
        var direction = _input.MoveDirection.X;
        var targetSpeed = direction * MaxSpeed;

        if (direction != 0)
            velocity.X = Mathf.MoveToward(velocity.X, targetSpeed, Acceleration * (float)delta);
        else
            velocity.X = Mathf.MoveToward(velocity.X, 0, Friction * (float)delta);
        
        return velocity;
    }
}