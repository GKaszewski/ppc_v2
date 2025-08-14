using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class ProgressiveDamageComponent : Node
{
    [Export] public HealthComponent HealthComponent { get; set; }
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public PlatformMovementComponent PlatformMovement { get; set; }
    [Export] public float MinJumpHeight { get; set; } = 60f;
    [Export] public float JumpReductionPercentage { get; set; } = 0.1f; // this is a percentage of the jump height per hit

    private float _maxHealth;
    private float _ogJumpHeight;

    public override void _Ready()
    {
        _maxHealth = HealthComponent.MaxHealth;
        HealthComponent.HealthChanged += OnHealthChanged;

        if (PlatformMovement != null)
        {
            _ogJumpHeight = PlatformMovement.JumpHeight;
        }
    }
    
    private void OnHealthChanged(float delta, float totalHealth)
    {
        var frame = GetDamageFrame();
        if (frame < 0 || frame >= Sprite.GetHframes()) return;
        
        Sprite.Frame = frame;
        if (PlatformMovement != null)
        {
            PlatformMovement.JumpHeight = GetJumpHeight();
        }
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

    private float GetJumpHeight()
    {
        if (PlatformMovement == null) return 0f;

        var jumpHeight = _ogJumpHeight;
        if (jumpHeight <= 0f) return 0f;
        
        var damageFrame = GetDamageFrame();
        if (damageFrame < 0 || damageFrame >= Sprite.GetHframes()) return jumpHeight;
        
        var reduction = JumpReductionPercentage * jumpHeight;
        var calculatedJumpHeight = jumpHeight - (damageFrame * reduction);
        return Mathf.Max(calculatedJumpHeight, MinJumpHeight);
    }
}