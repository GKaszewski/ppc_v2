using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GroundMovementAbility : MovementAbility
{
    [Export] public float Speed { get; set; } = 300.0f;
    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        var direction = _input.MoveDirection.X;

        if (direction != 0)
            velocity.X = direction * Speed;
        else
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
        
        return velocity;
    }
}