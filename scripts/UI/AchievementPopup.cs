using System.Threading.Tasks;
using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

[GlobalClass]
public partial class AchievementPopup : CanvasLayer
{
    [Export] public Label TitleLabel { get; set; }
    [Export] public Label DescriptionLabel { get; set; }
    [Export] public TextureRect IconRect { get; set; }
    [Export] public float DisplayDuration { get; set; } = 3.0f;

    private Control _container;
    private Vector2 _startPosition;
    private Vector2 _onScreenPosition;
    
    public override void _Ready()
    {
        _container = GetNode<Control>("Container");

        _startPosition = new Vector2(_container.Position.X, -_container.Size.Y - 50);
        _onScreenPosition = _container.Position;
        _container.Position = _startPosition;
    }
    
    public async Task ShowAchievement(AchievementResource achievement)
    {
        TitleLabel.Text = achievement.DisplayName;
        DescriptionLabel.Text = achievement.Description;
        IconRect.Texture = achievement.Icon;

        var tween = CreateTween();
        tween.TweenProperty(_container, "position", _onScreenPosition, 0.5f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        
        await ToSignal(tween, Tween.SignalName.Finished);

        await ToSignal(GetTree().CreateTimer(DisplayDuration), Timer.SignalName.Timeout);

        tween = CreateTween();
        tween.TweenProperty(_container, "position", _startPosition, 0.5f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.In);
        
        await ToSignal(tween, Tween.SignalName.Finished);
        
        QueueFree();
    }
}