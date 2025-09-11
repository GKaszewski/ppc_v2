using Godot;

namespace Mr.BrickAdventures.scripts.UI;

[GlobalClass]
public partial class FloatingText : Label
{
    [Export] public float Duration { get; set; } = 1f;
    [Export] public float FallDistance { get; set; } = 40f;
    [Export] public float HorizontalDrift { get; set; } = 15f;
    
    public void Show(string textToShow, Vector2 position, Color color)
    {
        Text = textToShow;
        GlobalPosition = position;
        Modulate = color;
        
        var rng = new RandomNumberGenerator();
        var horizontalOffset = rng.RandfRange(-HorizontalDrift, HorizontalDrift);

        var endPosition = GlobalPosition + new Vector2(horizontalOffset, FallDistance);

        var tween = CreateTween().SetParallel();

        tween.TweenProperty(this, "global_position", endPosition, Duration)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);
        
        tween.Chain().TweenProperty(this, "modulate:a", 0f, Duration * 0.5f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);
            
        tween.TweenCallback(Callable.From(QueueFree));
    }
}