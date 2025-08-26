using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class BrickThrowComponent : Node, ISkill
{
    [Export] public PackedScene BrickScene { get; set; }
    [Export] public float FireRate { get; set; } = 1.0f;
    [Export] public PlayerController PlayerController { get; set; }
    [Export] public ThrowInputResource ThrowInputBehavior { get; set; }
    
    private bool _canThrow = true;
    private Timer _timer;
    private SkillData _skillData;

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
        if (!_canThrow || PlayerController == null || BrickScene == null)
            return;

        var instance = BrickScene.Instantiate<Node2D>();
        var init = instance.GetNodeOrNull<ProjectileInitComponent>("ProjectileInitComponent");
        
        if (init != null && PlayerController.CurrentMovement is PlatformMovementComponent)
        {
            var @params = new ProjectileInitParams()
            {
                Position = PlayerController.GlobalPosition,
                Rotation = PlayerController.Rotation,
                Direction = PlayerController.CurrentMovement.LastDirection,
                PowerMultiplier = powerMultiplier,
            };
            
            init.Initialize(@params);
        }
        
        GetTree().CurrentScene.AddChild(instance);
        _canThrow = false;
        _timer.Start();
    }

    public void Initialize(Node owner, SkillData data)
    {
        PlayerController = owner as PlayerController;
        _skillData = data;

        ThrowInputBehavior = (ThrowInputResource)ThrowInputBehavior?.Duplicate();

        if (PlayerController == null)
        {
            GD.PushError("BrickThrowComponent: Owner is not a PlayerController.");
        }
        
        if (_skillData.Level > 0 && _skillData.Upgrades.Count >= _skillData.Level)
        {
            ApplyUpgrade(_skillData.Upgrades[_skillData.Level - 1]);
        }
    }

    public void Activate()
    {
        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested += ThrowBrick;
        SetProcessInput(true);
    }

    public void Deactivate()
    {
        if (ThrowInputBehavior != null) ThrowInputBehavior.ThrowRequested -= ThrowBrick;
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
    {
        foreach (var property in upgrade.Properties)
        {
            Set(property.Key, property.Value);
        }
    }
}