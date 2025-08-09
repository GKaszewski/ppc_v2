using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class FlashingComponent : Node
{
    [Export] public Node2D Sprite { get; set; }
    [Export] public float FlashDuration { get; set; } = 0.5f;
    [Export] public float FlashTime { get; set; } = 0.1f;
    [Export] public bool UseModulate { get; set; } = true;
    [Export] public HealthComponent HealthComponent { get; set; }

    private Tween _tween;

    public override void _Ready()
    {
        if (HealthComponent != null)
        {
            HealthComponent.HealthChanged += OnHealthChanged;
            HealthComponent.Death += OnDeath;
        }

        if (Sprite == null)
        {
            GD.PushError("FlashingComponent: Sprite node is not set.");
            return;
        }
    }

    public void StartFlashing()
    {
        if (Sprite == null) return;

        _tween?.Kill();

        _tween = CreateTween();
        _tween.SetParallel(true);

        var flashes = (int)(FlashDuration / FlashTime);
        for (var i = 0; i < flashes; i++)
        {
            if (UseModulate)
            {
                var opacity = i % 2 == 0 ? 1.0f : 0.3f;
                _tween.TweenProperty(Sprite, "modulate:a", opacity, FlashTime);
            }
            else
            {
                var visible = i % 2 == 0;
                _tween.TweenProperty(Sprite, "visible", visible, FlashTime);
            }
        }

        _tween.TweenCallback(Callable.From(StopFlashing));
    }

    public void StopFlashing()
    {
        if (UseModulate)
        {
            var modulateColor = Sprite.GetModulate();
            modulateColor.A = 1.0f;
            Sprite.SetModulate(modulateColor);
        }
        else
        {
            Sprite.SetVisible(true);
        }
    }

    private void OnHealthChanged(float delta, float totalHealth)
    {
        if (delta < 0f)
        {
            StartFlashing();
        }
    }
    
    private void OnDeath()
    {
        StopFlashing();
    }
}