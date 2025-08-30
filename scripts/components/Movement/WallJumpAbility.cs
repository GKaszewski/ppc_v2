using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class WallJumpAbility : MovementAbility
{
    [ExportGroup("Wall Jump Design")]
    [Export] public Vector2 WallJumpVelocity { get; set; } = new(500.0f, -350.0f);
    
    [ExportGroup("Wall Slide Feel")]
    [Export(PropertyHint.Range, "0.0, 1.0, 0.05")] public float WallSlideGravityMultiplier { get; set; } = 0.7f;
    [Export] public float MaxWallSlideSpeed { get; set; } = 150.0f;
    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        var isOnWall = _body.IsOnWall();

        if (isOnWall && !_body.IsOnFloor() && velocity.Y > 0f)
        {
            var gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
            var newYVelocity = velocity.Y + gravity * WallSlideGravityMultiplier * (float)delta;
            
            velocity.Y = Mathf.Min(newYVelocity, MaxWallSlideSpeed);
        }

        if (isOnWall && _input.JumpHeld)
        {
            var wallNormal = _body.GetWallNormal();
            
            velocity = new Vector2(wallNormal.X * WallJumpVelocity.X, WallJumpVelocity.Y);
        }
        
        return velocity;
    }
}