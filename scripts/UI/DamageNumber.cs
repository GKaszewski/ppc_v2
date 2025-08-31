using System.Globalization;
using Godot;

namespace Mr.BrickAdventures.scripts.UI;

[GlobalClass]
public partial class DamageNumber : Label
{
    [Export] public float Duration { get; set; } = 0.8f;
    [Export] public float FallDistance { get; set; } = 40f;
    [Export] public float HorizontalDrift { get; set; } = 15f;
    
    public void ShowDamage(float damageAmount, Vector2 position)
    {
        Text = Mathf.Round(damageAmount * 100f).ToString(CultureInfo.InvariantCulture);
        GlobalPosition = position;
        
        var rng = new RandomNumberGenerator();
        var horizontalOffset = rng.RandfRange(-HorizontalDrift, HorizontalDrift);

        var startPosition = GlobalPosition;
        var endPosition = GlobalPosition + new Vector2(horizontalOffset, FallDistance);

        var startColor = Colors.White;
        startColor.A = 1f;
        Modulate = startColor;
        
        var tween = CreateTween();
        tween.SetParallel();

        tween.TweenProperty(this, "global_position", endPosition, Duration)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.In);
            
        tween.Chain().TweenProperty(this, "modulate:a", 0f, Duration * 0.5f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        tween.TweenCallback(Callable.From(QueueFree));
    }
}