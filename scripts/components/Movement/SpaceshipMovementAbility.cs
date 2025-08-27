using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class SpaceshipMovementAbility : MovementAbility
{
    [Export] public float MaxSpeed { get; set; } = 200f;
    [Export] public float Acceleration { get; set; } = 100f;
    [Export] public float Friction { get; set; } = 50f;
    
    public override Vector2 ProcessMovement(Vector2 currentVelocity, double delta)
    {
        var inputVector = _input.MoveDirection;

        if (inputVector != Vector2.Zero)
        {
            currentVelocity = currentVelocity.MoveToward(inputVector * MaxSpeed, Acceleration * (float)delta);
        }
        else
        {
            currentVelocity = currentVelocity.MoveToward(Vector2.Zero, Friction * (float)delta);
        }

        return currentVelocity;
    }
}