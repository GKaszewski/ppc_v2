using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GravityAbility : MovementAbility
{
    [Export] public float FallGravityMultiplier { get; set; } = 2.0f;
    private float _gravity;

    public override void Initialize()
    {
        base.Initialize();
        _gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
    }

    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (_body.IsOnFloor()) return velocity;
        
        var gravityToApply = velocity.Y > 0 ? _gravity * FallGravityMultiplier : _gravity;
        velocity.Y += gravityToApply * (float)delta;
        
        return velocity;
    }
}