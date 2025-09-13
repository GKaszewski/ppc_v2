using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class JumpGfxComponent : Node2D
{
    [Export] public PackedScene ParticleScene { get; set; }
    [Export] public PlayerController Controller { get; set; }

    public override void _Ready()
    {
        if (Controller == null)
        {
            GD.PrintErr("JumpGfxComponent must have a reference to a PlayerController.");
            SetProcess(false);
            return;
        }
        
        Controller.JumpInitiated += OnJumpInitiated;
    }

    private void OnJumpInitiated()
    {
        SpawnGfx();
    }

    private void SpawnGfx()
    {
        var particleInstance = ParticleScene.Instantiate<GpuParticles2D>();
        particleInstance.GlobalPosition = GlobalPosition;
        GetTree().CurrentScene.AddChild(particleInstance);
        particleInstance.Emitting = true;
    }
}