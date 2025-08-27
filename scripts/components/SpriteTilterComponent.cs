using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class SpriteTilterComponent : Node
{
    [Export] public Node2D RotationTarget { get; set; }
    [Export(PropertyHint.Range, "0,45,1")] public float MaxTiltAngle { get; set; } = 10.0f;
    
    private CharacterBody2D _body;
    
    public override void _Ready()
    {
        _body = GetOwner<CharacterBody2D>();
        if (_body == null)
        {
            GD.PrintErr("SpriteTilterComponent must be a direct child of a CharacterBody2D.");
            SetProcess(false);
        }
        if (RotationTarget == null)
        {
            GD.PrintErr("SpriteTilterComponent needs a RotationTarget to be set in the inspector.");
            SetProcess(false);
        }
    }
    
    public override void _Process(double delta)
    {
        var targetAngleRad = 0.0f;
        var horizontalVelocity = _body.Velocity.X;

        if (horizontalVelocity > 0.1f)
            targetAngleRad = -Mathf.DegToRad(MaxTiltAngle);
        else if (horizontalVelocity < -0.1f) targetAngleRad = Mathf.DegToRad(MaxTiltAngle);
        else targetAngleRad = 0.0f;

        RotationTarget.Rotation = targetAngleRad;
    }
}