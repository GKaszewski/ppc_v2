using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class MovingPlatformComponent : AnimatableBody2D
{
    public enum LoopModeEnum { OneShot, Loop, PingPong }
    
    [Export] public Path2D Path { get; set; }
    [Export(PropertyHint.Range, "10, 1000, 1")] public float Speed { get; set; } = 100.0f;
    [Export] public LoopModeEnum LoopMode { get; set; } = LoopModeEnum.PingPong;
    [Export(PropertyHint.Range, "0, 10, 0.1")] public float WaitTime { get; set; } = 1.0f;
    
    private PathFollow2D _pathFollower;
    private Tween _tween;

    public override void _Ready()
    {
        if (Path == null)
        {
            GD.PrintErr("MovingPlatform: Path is not set. The platform will not move.");
            return;
        }
        
        _pathFollower = new PathFollow2D
        {
            Loop = false,
            Rotates = false
        };
        Path.AddChild(_pathFollower);
        
        StartMovement();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (_pathFollower == null) return;
        
        GlobalPosition = _pathFollower.GlobalPosition;
    }

    private void StartMovement()
    {
        _tween?.Kill();
        _tween = CreateTween();
        _tween.SetParallel(false);
        
        var pathLength =Path.Curve.GetBakedLength();
        var duration = pathLength / Speed;
        
        switch (LoopMode)
        {
            case LoopModeEnum.OneShot:
                _tween.TweenProperty(_pathFollower, "progress", pathLength, duration);
                break;

            case LoopModeEnum.Loop:
                _tween.TweenProperty(_pathFollower, "progress", pathLength, duration);
                _tween.TweenInterval(WaitTime);
                _tween.TweenCallback(Callable.From(() => _pathFollower.Progress = 0));
                _tween.SetLoops();
                break;

            case LoopModeEnum.PingPong:
                _tween.TweenProperty(_pathFollower, "progress", pathLength, duration);
                _tween.TweenInterval(WaitTime);
                _tween.TweenProperty(_pathFollower, "progress", 0, duration);
                _tween.TweenInterval(WaitTime);
                _tween.SetLoops();
                break;
        }
    }
}