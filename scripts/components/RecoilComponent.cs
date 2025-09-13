using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class RecoilComponent : Node
{
    [Export] public Node2D RecoilTarget { get; set; }
    [Export] public float RecoilDistance { get; set; } = 8f;
    [Export] public float RecoilDuration { get; set; } = 0.1f;
    
    private Vector2 _originalPosition;
    private Tween _recoilTween;
    private PeriodicShootingComponent _shootingComponent;

    public override void _Ready()
    {
        RecoilTarget ??= Owner as Node2D;
        if (RecoilTarget == null)
        {
            GD.PushError("RecoilComponent: RecoilTarget is null");
            SetProcess(false);
            return;
        }
        
        _originalPosition = RecoilTarget.Position;
        
        _shootingComponent = Owner.GetNodeOrNull<PeriodicShootingComponent>("PeriodicShootingComponent");
        if (_shootingComponent != null)
        {
            _shootingComponent.ShotFired += TriggerRecoil;
        }
    }

    public void TriggerRecoil(Vector2 shootDirection)
    {
        if (RecoilTarget == null) return;
        
        _recoilTween?.Kill();
        
        var recoilDirection = -shootDirection.Normalized();
        var recoilPosition = _originalPosition + recoilDirection * RecoilDistance;

        _recoilTween = CreateTween();
        _recoilTween.SetEase(Tween.EaseType.Out);
        _recoilTween.SetTrans(Tween.TransitionType.Cubic);
        
        _recoilTween.TweenProperty(RecoilTarget, "position", recoilPosition, RecoilDuration / 2);
        _recoilTween.TweenProperty(RecoilTarget, "position", _originalPosition, RecoilDuration / 2).SetDelay(RecoilDuration / 2);
    }
}