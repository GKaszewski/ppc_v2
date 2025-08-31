using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class CollapsableComponent : Node
{
    [Export] public Timer ToCollapseTimer { get; set; }
    [Export] public Timer ResetTimer { get; set; }
    [Export] public Sprite2D Sprite2D { get; set; }
    [Export] public CollisionShape2D CollisionShape { get; set; }
    [Export] public float CollapseTime { get; set; } = 0.5f;
    [Export] public float ResetTime { get; set; } = 0.5f;
    [Export] public float AnimationTime { get; set; } = 0.25f;

    public override void _Ready()
    {
        ResetTimers();
        
        ToCollapseTimer.Timeout += OnToCollapseTimerTimeout;
        ResetTimer.Timeout += OnResetTimerTimeout;
    }
    
    public void OnCollapsableDetectorBodyEntered(Node2D body)
    {
        ToCollapseTimer.Start();
    }

    public void OnCollapsableDetectorBodyExited(Node2D body)
    {
        var collapseTimeLeft = Mathf.Abs(ToCollapseTimer.TimeLeft - CollapseTime);
        if (collapseTimeLeft < (0.1f * CollapseTime))
        {
            ResetTimers();
        }
    }

    private void OnToCollapseTimerTimeout()
    {
        _ = Collapse();
    }

    private void OnResetTimerTimeout()
    {
        _ = Reactivate();
    }

    private async Task Collapse()
    {
        ToCollapseTimer.Stop();
        ToCollapseTimer.SetWaitTime(CollapseTime);

        var tween = CreateTween();
        tween.TweenProperty(Sprite2D, "modulate:a", 0f, AnimationTime);
        await ToSignal(tween, Tween.SignalName.Finished);
        
        CollisionShape?.CallDeferred("set_disabled", true);
        ResetTimer.Start();
    }

    private async Task Reactivate()
    {
        ResetTimer.Stop();
        ResetTimer.SetWaitTime(ResetTime);

        var tween = CreateTween();
        tween.TweenProperty(Sprite2D, "modulate:a", 1f, AnimationTime);
        await ToSignal(tween, Tween.SignalName.Finished);
        
        CollisionShape?.CallDeferred("set_disabled", false);
    }

    private void ResetTimers()
    {
        ToCollapseTimer.Stop();
        ToCollapseTimer.SetWaitTime(CollapseTime);
        
        ResetTimer.Stop();
        ResetTimer.SetWaitTime(ResetTime);
    }
}