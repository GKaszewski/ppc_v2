using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class PeriodicShootingComponent : Node
{
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public float ShootInterval { get; set; } = 1.0f;
    [Export] public Vector2 ShootDirection { get; set; } = Vector2.Right;
    [Export] public SideToSideMovementComponent SideToSideMovement { get; set; }
    [Export] public Node2D BulletSpawnRight { get; set; }
    [Export] public Node2D BulletSpawnLeft { get; set; }
    [Export] public float ShootingIntervalVariation { get; set; } = 0.0f;

    private Timer _timer;

    public override void _Ready()
    {
        SetupTimer();
    }

    public override void _Process(double delta)
    {
        if (SideToSideMovement == null) return;
        
        ShootDirection = SideToSideMovement.Direction != Vector2.Zero ? SideToSideMovement.Direction : Vector2.Right;
    }

    private void SetupTimer()
    {
        _timer = new Timer();
        _timer.WaitTime = GetShootInterval();
        _timer.OneShot = false;
        _timer.Autostart = true;
        _timer.Timeout += OnTimerTimeout;
        AddChild(_timer);
    }

    private void OnTimerTimeout()
    {
        Shoot();
        _timer.Start();
    }

    private double GetShootInterval()
    {
        if (ShootingIntervalVariation == 0f) return ShootInterval;
        
        var rng = new RandomNumberGenerator();
        return ShootInterval + rng.RandfRange(-ShootingIntervalVariation, ShootingIntervalVariation);
    }

    private void Shoot()
    {
        if (ShootDirection == Vector2.Zero) return;

        var root = Owner as Node2D;
        var bulletInstance = BulletScene.Instantiate<Node2D>();
        var launchComponent = bulletInstance.GetNodeOrNull<LaunchComponent>("LaunchComponent");
        var spawnPosition = ShootDirection == Vector2.Right ? BulletSpawnRight.GlobalPosition : BulletSpawnLeft.GlobalPosition;
        if (launchComponent != null)
        {
            launchComponent.InitialDirection = ShootDirection;
            launchComponent.SpawnPosition = spawnPosition;
            if (root != null) launchComponent.SpawnRotation = root.Rotation;
        }
        
        bulletInstance.Position = spawnPosition;
        GetTree().CurrentScene.AddChild(bulletInstance);
    }
}