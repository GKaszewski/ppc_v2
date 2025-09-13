using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class SquashAndStretchComponent : Node
{
    [Export] public Node2D TargetNode { get; set; }
    [Export(PropertyHint.Range, "0.1, 1.0, 0.01")] public float AnimationDuration { get; set; } = 0.25f;

    [ExportGroup("Effect Strength")]
    [Export(PropertyHint.Range, "1.0, 2.0, 0.05")] public float SquashFactor { get; set; } = 1.2f;
    [Export(PropertyHint.Range, "0.5, 1.0, 0.05")] public float StretchFactor { get; set; } = 0.8f;
    
    private Vector2 _originalScale;
    private Tween _tween;
    
    public override void _Ready()
    {
        TargetNode ??= Owner as Node2D;

        if (TargetNode == null)
        {
            GD.PrintErr("SquashAndStretchComponent: No valid TargetNode found. Disabling component.");
            SetProcess(false);
            return;
        }

        _originalScale = TargetNode.Scale;

        var shootingComponent = Owner.GetNodeOrNull<PeriodicShootingComponent>("PeriodicShootingComponent");
        if (shootingComponent != null)
        {
            shootingComponent.ShotFired += OnShotFired;
        }
        else
        {
            GD.PrintErr("SquashAndStretchComponent requires a PeriodicShootingComponent on the same owner to function.");
        }
    }

    private void OnShotFired(Vector2 shootDirection)
    {
        if (TargetNode == null) return;

        _tween?.Kill();

        Vector2 squashScale;
        Vector2 stretchScale;
        
        if (Mathf.Abs(shootDirection.X) > Mathf.Abs(shootDirection.Y))
        {
            squashScale = new Vector2(StretchFactor, SquashFactor) * _originalScale;
            stretchScale = new Vector2(SquashFactor, StretchFactor) * _originalScale;
        }
        else
        {
            squashScale = new Vector2(SquashFactor, StretchFactor) * _originalScale;
            stretchScale = new Vector2(StretchFactor, SquashFactor) * _originalScale;
        }
        
        _tween = CreateTween();
        _tween.SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out);
        
        var partDuration = AnimationDuration / 3.0f;

        _tween.TweenProperty(TargetNode, "scale", squashScale, partDuration);
        _tween.TweenProperty(TargetNode, "scale", stretchScale, partDuration);
        _tween.TweenProperty(TargetNode, "scale", _originalScale, partDuration);
    }
}