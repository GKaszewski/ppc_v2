using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class StompDamageComponent : Node
{
    [Export] public float Damage { get; set; } = 0.25f;
    [Export] public Area2D Area { get; set; }
    [Export] public PlayerController Root { get; set; }

    public override void _Ready()
    {
        Area.BodyEntered += OnBodyEntered;
    }

    private async void OnBodyEntered(Node2D body)
    {
        var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");
        if (health == null) return;
        
        var cannotStompComponent = body.GetNodeOrNull<CannotStompComponent>("CannotStompComponent");
        if (cannotStompComponent != null) return;

        if (!(Root.GlobalPosition.Y < body.GlobalPosition.Y)) return;
        
        var velocity = Root.PreviousVelocity;
        if (!(velocity.Y > 0f)) return;
        
        DealDamage(health);
                
        var damageComponent = body.GetNodeOrNull<DamageComponent>("DamageComponent");
        if (damageComponent == null) return;
                
        damageComponent.SetProcess(false);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        damageComponent.SetProcess(true);
    }

    private void DealDamage(HealthComponent target)
    {
        target.DecreaseHealth(Damage);
    }
}