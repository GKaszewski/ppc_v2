using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class HitComponent : Node
{
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public HealthComponent Health { get; set; }
    [Export] public float HitDuration { get; set; } = 0.1f;
    [Export] public GpuParticles2D HitFx { get; set; }
    [Export] public bool FlashMode { get; set; } = true;

    public override void _Ready()
    {
        if (Health != null)
        {
            Health.HealthChanged += OnHealthChange;
            Health.Death += OnDeath;
        }

        if (Sprite == null)
        {
            GD.PushError("HitComponent: Sprite is null");
            return;
        }

        if (Sprite.Material != null && FlashMode)
        {
            Sprite.Material = (Material)Sprite.Material.Duplicate();
        }
    }

    private void Activate()
    {
        if (!FlashMode) return;

        Sprite.SetInstanceShaderParameter("enabled", true);
    }

    private void Deactivate()
    {
        if (!FlashMode) return;
        
        Sprite.SetInstanceShaderParameter("enabled", false);
    }

    private async void OnHealthChange(float delta, float totalHealth)
    {
        if (!(delta < 0f)) return;
        
        Activate();
        await ToSignal(GetTree().CreateTimer(HitDuration), Timer.SignalName.Timeout);
        Deactivate();
            
        if (totalHealth > 0f && delta < 0f)
        {
            HandleHitFx();
        }
    }

    private async void OnDeath()
    {
        Activate();
        await ToSignal(GetTree().CreateTimer(HitDuration), Timer.SignalName.Timeout);
        Deactivate();
    }

    private void HandleHitFx()
    {
        if (HitFx == null) return;
        
        HitFx.Restart();
        HitFx.Emitting = true;
    }
}