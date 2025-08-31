using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class FireEffectComponent : Node
{
    [Export] public HealthComponent Health { get; set; }
    [Export] public StatusEffectComponent StatusEffectComponent { get; set; }
    [Export] public GpuParticles2D FireFX { get; set; }

    private StatusEffectDataResource _data = null;
    private bool _shouldDealDamage = false;
    private double _timeElapsed = 0f;

    public override void _Ready()
    {
        if (Health == null)
        {
            Health = GetNode<HealthComponent>("HealthComponent");
        }
        if (StatusEffectComponent == null)
        {
            StatusEffectComponent = GetNode<StatusEffectComponent>("StatusEffectComponent");
        }
        
        if (Health == null) 
        {
            GD.PushError("FireEffectComponent: HealthComponent is not set.");
            return;
        }
        if (StatusEffectComponent == null) 
        {
            GD.PushError("FireEffectComponent: StatusEffectComponent is not set.");
            return;
        }
        
        StatusEffectComponent.EffectApplied += OnEffectApplied;
        StatusEffectComponent.EffectRemoved += OnEffectRemoved;
    }

    public override void _Process(double delta)
    {
        if (!_shouldDealDamage || _data == null || Health == null) return;

        _timeElapsed += delta;
        if (_timeElapsed >= 1f)
        {
            Health.DecreaseHealth(_data.DamagePerSecond);
            _timeElapsed = 0f;
        }
    }

    private void OnEffectApplied(StatusEffect statusEffect)
    {
        if (statusEffect.EffectData.Type != StatusEffectType.Fire) return;
        
        _data = statusEffect.EffectData;
        _shouldDealDamage = true;
        FireFX?.SetEmitting(true);
    }
    
    private void OnEffectRemoved(StatusEffectType type)
    {
        if (type != StatusEffectType.Fire) return;
        
        _shouldDealDamage = false;
        _data = null;
        FireFX?.SetEmitting(false);
    }
}