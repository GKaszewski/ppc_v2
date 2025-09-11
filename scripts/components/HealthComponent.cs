using System.Threading.Tasks;
using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class HealthComponent : Node2D
{
    [Export] public float Health { get; set; } = 1.0f;
    [Export] public float MaxHealth { get; set; } = 1.0f;
    [Export] public AudioStreamPlayer2D HurtSfx { get; set; }
    [Export] public AudioStreamPlayer2D HealSfx { get; set; }
    
    [Signal] public delegate void HealthChangedEventHandler(float delta, float totalHealth);
    [Signal] public delegate void DeathEventHandler();
    
    private FloatingTextManager _floatingTextManager;

    public override void _Ready()
    {
        _floatingTextManager = GetNode<FloatingTextManager>("/root/FloatingTextManager");
    }
    
    public void SetHealth(float newValue)
    {
        _ = ApplyHealthChange(newValue);
    }
    
    public void IncreaseHealth(float delta)
    {
        _ = ApplyHealthChange(Health + delta);
    }

    public void DecreaseHealth(float delta)
    {
        _ = ApplyHealthChange(Health - delta);
    }

    public float GetDelta(float newValue) => newValue - Health;

    private async Task ApplyHealthChange(float newHealth, bool playSfx = true)
    {
        newHealth = Mathf.Clamp(newHealth, 0.0f, MaxHealth);
        var delta = newHealth - Health;

        if (delta == 0.0f)
            return;
        
        if (delta < 0.0f)
            _floatingTextManager?.ShowDamage(Mathf.Abs(delta), GlobalPosition);
        else
            _floatingTextManager?.ShowHeal(delta, GlobalPosition);

        if (playSfx)
        {
            if (delta > 0f && HealSfx != null)
            {
                HealSfx.Play();
            }
            else if (delta < 0f && HurtSfx != null)
            {
                HurtSfx.Play();
                await HurtSfx.ToSignal(HurtSfx, AudioStreamPlayer2D.SignalName.Finished);
            }
        }
        
        Health = newHealth;

        if (Health <= 0f)
        {
            EmitSignalDeath();
        }
        else
        {
            EmitSignalHealthChanged(delta, Health);
        }
    }
}