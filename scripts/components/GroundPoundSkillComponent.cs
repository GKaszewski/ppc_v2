using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GroundPoundSkillComponent : Node, ISkill
{
    [Export] public float PoundForce { get; set; } = 1200f;
    [Export] public PackedScene ShockwaveScene { get; set; }
    
    private PlayerController _player;
    private PlayerInputHandler _input;
    private bool _isPounding = false;
    
    public void Initialize(Node owner, SkillData data)
    {
        _player = owner as PlayerController;
        if (_player != null)
        {
            _input = _player.GetNode<PlayerInputHandler>("PlayerInputHandler");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null || _input == null)
        {
            return;
        }
        
        // Check if we just landed from a ground pound to create the shockwave.
        if (_isPounding && _player.IsOnFloor())
        {
            _isPounding = false;
            if (ShockwaveScene != null)
            {
                var shockwave = ShockwaveScene.Instantiate<Node2D>();
                _player.GetParent()?.AddChild(shockwave);
                shockwave.GlobalPosition = _player.GlobalPosition;
            }
        }

        // Check to initiate a ground pound. The player must be in the air.
        if (_input.DownHeld && !_player.IsOnFloor() && !_isPounding)
        {
            // Apply a strong downward force, zeroing out horizontal movement.
            _player.Velocity = new Vector2(0, PoundForce);
            _isPounding = true;
        }
    }
    
    public void Activate() 
    {
        SetPhysicsProcess(true);
    }
    
    public void Deactivate() 
    {
        SetPhysicsProcess(false);
        _isPounding = false;
    }

    public void ApplyUpgrade(SkillUpgrade upgrade) { }
}