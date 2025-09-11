using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class KnockbackComponent : Node
{
    [Export] public float KnockbackForce { get; set; } = 400f;
    [Export] public float KnockbackDuration { get; set; } = 0.2f; // Duration in seconds
    
    /// <summary>
    /// Applies a knockback force to a target body, pushing it away from a source.
    /// </summary>
    /// <param name="target">The CharacterBody2D to apply the knockback to.</param>
    /// <param name="source">The Node2D causing the knockback (e.g., the enemy, the cactus).</param>
    public void ApplyKnockback(CharacterBody2D target, Node2D source)
    {
        if (target == null || source == null)
        {
            return;
        }

        var direction = (target.GlobalPosition - source.GlobalPosition).Normalized();
        
        if (direction == Vector2.Zero)
        {
            direction = Vector2.Up;
        }

        target.Velocity = direction * KnockbackForce;
        
        var tween = CreateTween();
        tween.TweenProperty(target, "velocity", Vector2.Zero, KnockbackDuration)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);
    }
}