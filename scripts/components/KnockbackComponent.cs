using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class KnockbackComponent : Node
{
    [Export] public CharacterBody2D Body { get; set; }
    [Export] public float KnockbackForce { get; set; } = 25f;
    [Export] public HealthComponent HealthComponent { get; set; }

    private bool _knockbackMode = false;
    private int _knockbackFrames = 0;

    public override void _Ready()
    {
        HealthComponent.HealthChanged += OnHealthChanged;
    }

    public override void _Process(double delta)
    {
        if (_knockbackMode) _knockbackFrames++;

        if (_knockbackFrames <= 1) return;
        
        _knockbackMode = false;
        _knockbackFrames = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_knockbackMode) ApplyKnockback((float)delta);
    }
    
    private void OnHealthChanged(float delta, float totalHealth)
    {
        if (totalHealth <= 0f || delta >= 0f) return;
        
        _knockbackMode = true;
    }

    private void ApplyKnockback(float delta)
    {
        var velocity = Body.Velocity.Normalized();
        var knockbackDirection = new Vector2(Mathf.Sign(velocity.X), 0.4f);
        var knockbackVector = -knockbackDirection * KnockbackForce * delta;
        Body.Velocity += knockbackVector;
    }
}