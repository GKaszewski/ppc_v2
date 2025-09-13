using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class JumpPadComponent : Node
{
    [Export] public float JumpForce { get; set; } = 10f;
    [Export] public Area2D Area { get; set; }
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public int StartAnimationIndex { get; set; } = 0;
    [Export] public float AnimationDuration { get; set; } = 0.5f;
    [Export] public GpuParticles2D Particles { get; set; }

    public override void _Ready()
    {
        Area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        var canBeLaunched = body.GetNodeOrNull<CanBeLaunchedComponent>("CanBeLaunchedComponent");
        if (canBeLaunched == null) return;

        if (body is not PlayerController player) return;
        
        _ = HandleLaunchPadAnimation();
        player.Velocity = new Vector2(player.Velocity.X, -JumpForce);
        player.EmitSignal(PlayerController.SignalName.JumpInitiated);
        Particles?.Restart();
    }

    private async Task HandleLaunchPadAnimation()
    {
        if (Sprite == null) return;

        var timer = GetTree().CreateTimer(AnimationDuration);
        Sprite.Frame = StartAnimationIndex + 1;
        await ToSignal(timer, Timer.SignalName.Timeout);
        Sprite.Frame = StartAnimationIndex;
    }
}