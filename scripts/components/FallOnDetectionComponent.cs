using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class FallOnDetectionComponent : Node2D
{
    [Export] public Area2D DetectionArea { get; set; }
    [Export] public RigidBody2D TargetBody { get; set; }
    [Export] public float FallDelay { get; set; } = 0.2f;
    
    private bool _hasTriggered = false;

    public override void _Ready()
    {
        if (DetectionArea == null)
        {
            GD.PrintErr("FallOnDetectionComponent: DetectionArea is not set.");
            SetProcess(false);
            return;
        }
        if (TargetBody == null)
        {
            GD.PrintErr("FallOnDetectionComponent: TargetBody is not set.");
            SetProcess(false);
            return;
        }

        DetectionArea.BodyEntered += OnBodyEntered;
    }
    
    private async void OnBodyEntered(Node2D body)
    {
        if (_hasTriggered) return;
        _hasTriggered = true;
        
        if (FallDelay > 0)
        {
            await ToSignal(GetTree().CreateTimer(FallDelay), Timer.SignalName.Timeout);
        }

        if (IsInstanceValid(TargetBody))
        {
            TargetBody.GravityScale = 1.0f;
        }
    }
}