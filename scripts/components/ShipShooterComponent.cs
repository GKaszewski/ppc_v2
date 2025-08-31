using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class ShipShooterComponent : Node
{
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public float FireRate { get; set; } = 0.2f;
    [Export] public Marker2D BulletSpawn { get; set; }
    [Export] public AudioStreamPlayer2D ShootSfx { get; set; }

    private bool _canShoot = true;

    public override void _Ready()
    {
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("attack") && _canShoot)
        {
            _ = Shoot();
        }
    }

    private async Task Shoot()
    {
        if (!_canShoot) return;

        var bullet = BulletScene.Instantiate<Node2D>();
        var init = bullet.GetNodeOrNull<ProjectileInitComponent>("ProjectileInitComponent");
        init?.Initialize(new ProjectileInitParams()
        {
            Position = BulletSpawn.GlobalPosition,
        });

        GetTree().CurrentScene.AddChild(bullet);
        ShootSfx?.Play();
        
        _canShoot = false;
        await ToSignal(GetTree().CreateTimer(FireRate), Timer.SignalName.Timeout);
        _canShoot = true;
    }

    private void OnShipEntered()
    {
        _canShoot = true;
        SetProcess(true);
    }

    private void OnShipExited()
    {
        _canShoot = false;
        SetProcess(false);
        ShootSfx?.Stop();
    }
}