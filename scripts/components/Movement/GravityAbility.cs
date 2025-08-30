using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GravityAbility : MovementAbility
{
    public float AscendGravity { get; set; }
    public float DescendGravity { get; set; }
    
    private float _gravity;

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        _gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
    }

    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (_body.IsOnFloor()) return velocity;
        
        var gravityToApply = velocity.Y < 0 ? AscendGravity : DescendGravity;
        velocity.Y += gravityToApply * (float)delta;
        
        return velocity;
    }
}