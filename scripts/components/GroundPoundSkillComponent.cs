using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GroundPoundSkillComponent : SkillComponentBase
{
    [Export] public float PoundForce { get; set; } = 1200f;
    [Export] public PackedScene ShockwaveScene { get; set; }

    private PlayerInputHandler _input;
    private bool _isPounding = false;

    public override void Initialize(Node owner, SkillData data)
    {
        base.Initialize(owner, data);
        if (Player != null)
        {
            _input = Player.GetNode<PlayerInputHandler>("PlayerInputHandler");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Player == null || _input == null)
        {
            return;
        }

        // Check if we just landed from a ground pound to create the shockwave.
        if (_isPounding && Player.IsOnFloor())
        {
            _isPounding = false;
            if (ShockwaveScene != null)
            {
                var shockwave = ShockwaveScene.Instantiate<Node2D>();
                Player.GetParent()?.AddChild(shockwave);
                shockwave.GlobalPosition = Player.GlobalPosition;
            }
        }

        // Check to initiate a ground pound. The player must be in the air.
        if (_input.DownHeld && !Player.IsOnFloor() && !_isPounding)
        {
            // Apply a strong downward force, zeroing out horizontal movement.
            Player.Velocity = new Vector2(0, PoundForce);
            _isPounding = true;
        }
    }

    public override void Activate()
    {
        SetPhysicsProcess(true);
    }

    public override void Deactivate()
    {
        SetPhysicsProcess(false);
        _isPounding = false;
    }
}