using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PlayerSfxComponent : Node
{
    [Export] public AudioStreamPlayer2D JumpSfx { get; set; }

    private PlayerController _controller;

    public override void _Ready()
    {
        _controller = GetOwner<PlayerController>();
        if (_controller == null)
        {
            GD.PrintErr("PlayerSfxComponent must be a child of a PlayerController.");
            SetProcess(false);
        }
        
        _controller.JumpInitiated += OnJumpInitiated;
    }

    private void OnJumpInitiated()
    {
        if (JumpSfx is { Playing: false })
            JumpSfx.Play();
    }
}