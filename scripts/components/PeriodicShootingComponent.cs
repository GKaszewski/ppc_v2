using System;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PeriodicShootingComponent : Node
{
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public float ShootInterval { get; set; } = 1.0f;
    [Export] public Vector2 ShootDirection { get; set; } = Vector2.Right;
    [Export] public Node2D BulletSpawnPointRight { get; set; }
    [Export] public Node2D BulletSpawnPointLeft { get; set; }
    [Export] public float ShootingIntervalVariation { get; set; } = 0.0f;

    private Timer _timer;
    private RandomNumberGenerator _rng;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        SetupTimer();
    }

    private void SetupTimer()
    {
        _timer = new Timer
        {
            WaitTime = GetNextShootInterval(),
            OneShot = false,
            Autostart = true
        };
        _timer.Timeout += OnTimerTimeout;
        AddChild(_timer);
    }

    private void OnTimerTimeout()
    {
        Shoot();
        _timer.WaitTime = GetNextShootInterval();
    }

    private double GetNextShootInterval()
    {
        if (ShootingIntervalVariation <= 0f)
        {
            return ShootInterval;
        }
        
        return Math.Max(0.01, ShootInterval + _rng.RandfRange(-ShootingIntervalVariation, ShootingIntervalVariation));
    }

    private void Shoot()
    {
        if (BulletScene == null)
        {
            GD.PushError("PeriodicShootingComponent: BulletScene is not set.");
            return;
        }
        
        if (ShootDirection == Vector2.Zero) return;
        
        var spawnNode = (ShootDirection.X >= 0 || BulletSpawnPointLeft == null) 
            ? BulletSpawnPointRight 
            : BulletSpawnPointLeft;
        
        if (spawnNode == null)
        {
            GD.PrintErr("PeriodicShootingComponent: A suitable bullet spawn point is not set.");
            return;
        }

        var spawnPosition = spawnNode.GlobalPosition;
        var owner = Owner as Node2D;
        var ownerRotation = owner?.Rotation ?? 0f;
        
        var bulletInstance = BulletScene.Instantiate<Node2D>();
        if (bulletInstance.GetNodeOrNull<LaunchComponent>("LaunchComponent") is { } launchComponent)
        {
            launchComponent.InitialDirection = ShootDirection;
            launchComponent.SpawnPosition = spawnPosition;
            launchComponent.SpawnRotation = ownerRotation;
        }
        
        bulletInstance.GlobalPosition = spawnPosition;
        GetTree().CurrentScene.AddChild(bulletInstance);
    }
}