using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class ExplosiveComponent : Node2D
{
    [Export] public DamageComponent Damage { get; set; }
    [Export] public Area2D Area { get; set; }
    [Export] public Area2D ExplodeArea { get; set; }
    [Export] public PackedScene ExplosionEffect { get; set; }
    [Export] public float TimeToExplode { get; set; } = 9f;
    
    [Signal] public delegate void OnExplosionEventHandler(Node2D body);

    private Timer _timer;

    public override void _Ready()
    {
        if (Damage != null)
        {
            GD.PushError("ExplosiveComponent: DamageComponent is not set.");
            return;
        }

        if (ExplodeArea != null)
        {
            GD.PushError("ExplosiveComponent: ExplodeArea is not set.");
            return;
        }
        
        Area.BodyEntered += OnAreaBodyEntered;
        Area.AreaEntered += OnAreaAreaEntered;
    }

    private void OnAreaAreaEntered(Area2D area)
    {
        Explode();
    }

    private void OnAreaBodyEntered(Node2D body)
    {
        Explode();
    }

    private void PrepareTimer()
    {
        _timer = new Timer();
        _timer.SetWaitTime(TimeToExplode);
        _timer.OneShot = true;
        _timer.Autostart = true;
        _timer.Timeout += Explode;
        AddChild(_timer);
    }

    private void Explode()
    {
        _timer.Stop();

        if (ExplosionEffect != null)
        {
            var explosionInstance = ExplosionEffect.Instantiate<GpuParticles2D>();
            if (Owner is Node2D root) explosionInstance.SetGlobalPosition(root.GlobalPosition);
            GetTree().CurrentScene.AddChild(explosionInstance);
            explosionInstance.SetEmitting(true);
        }

        var bodies = ExplodeArea.GetOverlappingBodies();
        foreach (var body in bodies)
        {
            var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");
            if (Damage != null && health != null)
            {
                Damage.DealDamage(health);
            }
            
            EmitSignalOnExplosion(body);
        }
        
        Owner.QueueFree();
    }
}