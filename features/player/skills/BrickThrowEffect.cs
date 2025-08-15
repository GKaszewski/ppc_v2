using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.features.player.skills;

[Meta(typeof(IAutoNode))]
public partial class BrickThrowEffect : Node, ISkillEffect
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] public PackedScene BrickScene { get; set; } = null!;
    [Export] public float FireRate { get; set; } = 0.35f;
    [Export] public string FireAction { get; set; } = "fire";
    
    [Node("%Player")] private PlayerController Player { get; set; } = null!;
    
    private Timer _cooldown = null!;
    private bool _readyToFire = true;
    private int _level = 1;
    
    public void OnReady() {
        _cooldown = new Timer { OneShot = true, WaitTime = FireRate, Autostart = false };
        AddChild(_cooldown);
        _cooldown.Timeout += () => _readyToFire = true;
    }
    
    public override void _Input(InputEvent e) {
        if (!_readyToFire) return;
        if (e.IsActionPressed(FireAction)) Fire();
    }
    
    public void Configure(string id, int level) {
        _level = Mathf.Max(1, level);
        _cooldown?.Stop();
        _cooldown.WaitTime = GetFireRate();
    }
    
    private void Fire() {
        if (BrickScene == null || Player == null) return;

        var inst = BrickScene.Instantiate<Node2D>();
        var init = inst.GetNodeOrNull<ProjectileInitComponent>("ProjectileInitComponent");

        var dir = Player.CurrentMovement is PlatformMovementComponent pm
            ? pm.LastDirection
            : Vector2.Right;

        var power = GetPowerMultiplier();
        init?.Initialize(new ProjectileInitParams {
            Position = Player.GlobalPosition,
            Rotation = dir.Angle(),
            Direction = dir,
            PowerMultiplier = power
        });

        GetTree().CurrentScene.AddChild(inst);

        _readyToFire = false;
        _cooldown.Start();
    }
    
    private float GetFireRate() {
        return Mathf.Max(0.1f, FireRate * (1.0f - 0.1f * (_level - 1)));
    }
    
    private float GetPowerMultiplier() {
        return 1.0f + 0.15f * (_level - 1);
    }
}