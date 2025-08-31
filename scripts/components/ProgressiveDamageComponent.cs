using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class ProgressiveDamageComponent : Node
{
    [Export] public HealthComponent HealthComponent { get; set; }
    [Export] public Sprite2D Sprite { get; set; }

    private float _maxHealth;
    private float _ogJumpHeight;

    public override void _Ready()
    {
        _maxHealth = HealthComponent.MaxHealth;
        HealthComponent.HealthChanged += OnHealthChanged;
    }
    
    private void OnHealthChanged(float delta, float totalHealth)
    {
        var frame = GetDamageFrame();
        if (frame < 0 || frame >= Sprite.GetHframes()) return;
        
        Sprite.Frame = frame;
    }

    private int GetDamageFrame()
    {
        if (Sprite == null || HealthComponent == null) return 0;

        var framesCount = Sprite.GetHframes();
        if (framesCount == 0) return 0;
        
        var currentHealth = HealthComponent.Health;
        var healthRatio = currentHealth / _maxHealth;
        return (int)(framesCount * (1f - healthRatio));
    }
}