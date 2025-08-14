using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class FadeAwayComponent : Node
{
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public float FadeDuration { get; set; } = 1f;
    [Export] public float Speed { get; set; } = 10f;
    [Export] public Vector2 Direction { get; set; } = Vector2.Up;
    [Export] public Area2D Area { get; set; }

    public override void _Ready()
    {
        Area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        _ = FadeAway();
    }

    private async Task FadeAway()
    {
        var tween = CreateTween().SetParallel(true);
        tween.TweenProperty(Sprite, "modulate:a", 0f, FadeDuration);
        tween.TweenProperty(Sprite, "position", Sprite.Position + (Direction * Speed), FadeDuration); 
        await ToSignal(tween, Tween.SignalName.Finished);
        Owner.QueueFree();
    }
}