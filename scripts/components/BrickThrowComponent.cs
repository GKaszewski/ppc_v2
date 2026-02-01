using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BrickThrowComponent : SkillComponentBase
{
    [Export] public PackedScene BrickScene { get; set; }
    [Export] public float FireRate { get; set; } = 1.0f;
    [Export] public ThrowInputResource ThrowInputBehavior { get; set; }

    private bool _canThrow = true;
    private Timer _timer;

    public override void _Ready()
    {
        SetupTimer();
        _canThrow = true;
    }

    public override void _ExitTree()
    {
        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested -= ThrowBrick;
        if (_timer != null)
        {
            _timer.Timeout -= OnTimerTimeout;
            _timer.QueueFree();
        }
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
        _timer = new Timer();
        _timer.WaitTime = FireRate;
        _timer.OneShot = false;
        _timer.Autostart = false;
        _timer.Timeout += OnTimerTimeout;
        AddChild(_timer);
    }

    private void OnTimerTimeout()
    {
        _canThrow = true;
    }

    private void ThrowBrick(float powerMultiplier = 1f)
    {
        if (!_canThrow || Player == null || BrickScene == null)
            return;

        var instance = BrickScene.Instantiate<Node2D>();
        var init = instance.GetNodeOrNull<ProjectileInitComponent>("ProjectileInitComponent");

        if (init != null)
        {
            var @params = new ProjectileInitParams()
            {
                Position = Player.GlobalPosition,
                Rotation = Player.Rotation,
                Direction = Player.LastDirection,
                PowerMultiplier = powerMultiplier,
            };

            init.Initialize(@params);
        }

        GetTree().CurrentScene.AddChild(instance);
        _canThrow = false;
        _timer.Start();
    }

    public override void Initialize(Node owner, SkillData data)
    {
        base.Initialize(owner, data);

        ThrowInputBehavior = (ThrowInputResource)ThrowInputBehavior?.Duplicate();

        if (Data.Level > 0 && Data.Upgrades.Count >= Data.Level)
        {
            ApplyUpgrade(Data.Upgrades[Data.Level - 1]);
        }
    }

    public override void Activate()
    {
        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested += ThrowBrick;
        SetProcessInput(true);
    }

    public override void Deactivate()
    {
        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested -= ThrowBrick;
    }

    public override void ApplyUpgrade(SkillUpgrade upgrade)
    {
        foreach (var property in upgrade.Properties)
        {
            Set(property.Key, property.Value);
        }
    }
}