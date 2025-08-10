using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class DestroyableComponent : Node2D
{
    [Export] public HealthComponent Health { get; set; }
    [Export] public PackedScene DestroyEffect { get; set; }

    public override void _Ready()
    {
        if (Health == null)
        {
            GD.PushError("DestroyableComponent: HealthComponent is not set.");
            return;
        }
        
        Health.Death += OnHealthDeath;
    }

    private void OnHealthDeath()
    {
        if (DestroyEffect == null)
        {
            Owner.QueueFree();
            return;
        }

        var effect = DestroyEffect.Instantiate<Node2D>();
        Health.GetParent().AddChild(effect);
        effect.SetGlobalPosition(Health.GlobalPosition);
        Owner.QueueFree();
    }
    
}