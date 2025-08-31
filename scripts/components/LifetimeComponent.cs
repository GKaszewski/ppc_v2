using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class LifetimeComponent : Node
{
    [Export] public float LifeTime { get; set; } = 5.0f;
    
    private Timer _lifetimeTimer;

    public override void _Ready()
    {
        _lifetimeTimer = new Timer();
        _lifetimeTimer.WaitTime = LifeTime;
        _lifetimeTimer.OneShot = true;
        _lifetimeTimer.Autostart = true;
        _lifetimeTimer.Timeout += OnLifetimeTimeout;
        
        AddChild(_lifetimeTimer);
        _lifetimeTimer.Start();
    }
    
    private void OnLifetimeTimeout()
    {
        Owner.QueueFree();
    }
}