using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class HealComponent : Node
{
    [Export] public GpuParticles2D HealFx { get; set; }
    [Export] public CollectableComponent Collectable { get; set; }

    public override void _Ready()
    {
        if (Collectable == null)
        {
            GD.PushError("HealComponent: Collectable is not set.");
            return;
        }

        // Register check to prevent collecting when at full health
        Collectable.CanCollect = CanCollectHealth;
        Collectable.Collected += OnCollected;
    }

    private bool CanCollectHealth(Node2D body)
    {
        var healthComponent = body.GetNodeOrNull<HealthComponent>("HealthComponent");
        if (healthComponent == null) return true; // Allow collection if no health component

        // Prevent collection if already at full health
        return healthComponent.Health < healthComponent.MaxHealth;
    }

    private void OnCollected(float amount, CollectableType type, Node2D body)
    {
        if (type != CollectableType.Health) return;

        if (Collectable == null) return;

        var healthComponent = body.GetNodeOrNull<HealthComponent>("HealthComponent");
        if (healthComponent == null) return;

        healthComponent.IncreaseHealth(amount);
        if (HealFx != null)
        {
            PlayHealFx();
        }

        Owner.QueueFree();
    }

    private void PlayHealFx()
    {
        if (HealFx == null) return;

        HealFx.Restart();
        HealFx.Emitting = true;
    }
}