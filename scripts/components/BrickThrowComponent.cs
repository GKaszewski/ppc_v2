using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class BrickThrowComponent : Node
{
    [Export] public PackedScene BrickScene { get; set; }
    [Export] public float FireRate { get; set; } = 1.0f;
    [Export] public PlayerController PlayerController { get; set; }
    [Export] public ThrowInputResource ThrowInputBehavior { get; set; }
    
    private bool _canThrow = true;
    private Timer _timer;

    public override void _Ready()
    {
        SetupTimer();
        _canThrow = true;

        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested += ThrowBrick;
    }

    public override void _Input(InputEvent @event)
    {
        ThrowInputBehavior?.ProcessInput(@event);
    }

    public override void _Process(double delta)
    {
        ThrowInputBehavior?.Update(delta);
    }

    private void SetupTimer()
    {
        _timer.WaitTime = FireRate;
        _timer.OneShot = false;
        _timer.Autostart = false;
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        _canThrow = true;
    }

    private void ThrowBrick(float powerMultiplier = 1f)
    {
        if (!_canThrow || PlayerController == null || BrickScene == null)
            return;

        var instance = BrickScene.Instantiate<Node2D>();
        var init = instance.GetNodeOrNull<ProjectileInitComponent>("ProjectileInitComponent");
        if (init != null && PlayerController.CurrentMovement is PlatformMovementComponent)
        {
            init.Initialize(new ProjectileInitParams()
            {
                Position = PlayerController.GlobalPosition,
                Rotation = PlayerController.Rotation,
                Direction = PlayerController.CurrentMovement.LastDirection,
                PowerMultiplier = powerMultiplier,
            });
        }
        
        GetTree().CurrentScene.AddChild(instance);
        _canThrow = false;
        _timer.Start();
    }
}