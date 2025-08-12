using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class ChaseLevelComponent : Node
{
    [Export] public float ChaseSpeed { get; set; } = 200.0f;
    [Export] public Marker2D ChaseTarget { get; set; }
    [Export] public GodotObject PhantomCamera { get; set; }
    [Export] public float MinimumDistance { get; set; } = 10f;

    [Signal]
    public delegate void ChaseStartedEventHandler();

    [Signal]
    public delegate void ChaseStoppedEventHandler();

    private bool _isChasing = false;
    private Node2D _previousCameraFollowTarget = null;

    public override void _Process(double delta)
    {
        if (!_isChasing) return;
        if (ChaseTarget == null) return;

        if (CheckIfReachedTarget())
        {
            StopChasing();
            return;
        }
        
        var targetPosition = ChaseTarget.GlobalPosition;
        
        if (Owner is not Node2D root) return;
        
        var direction = (targetPosition - root.GlobalPosition).Normalized();
        root.GlobalPosition += direction * ChaseSpeed * (float)delta;
    }

    public void OnShipEntered()
    {
        if (ChaseTarget == null || PhantomCamera == null)
            return;

        if (_isChasing) return;

        _previousCameraFollowTarget = (Node2D)PhantomCamera.Call("get_follow_target");
        PhantomCamera.Call("set_follow_target", Owner as Node2D);
        EmitSignalChaseStarted();
        _isChasing = true;
    }

    public void OnShipExited()
    {
        StopChasing();
    }

    private bool CheckIfReachedTarget()
    {
        if (ChaseTarget == null)
            return false;
        
        if (Owner is not Node2D root) return false;
        
        var targetPosition = ChaseTarget.GlobalPosition;
        var currentPosition = root.GlobalPosition;
        return currentPosition.DistanceTo(targetPosition) <= MinimumDistance;

    }

    private void StopChasing()
    {
        if (PhantomCamera == null) return;

        PhantomCamera.Call("set_follow_target", _previousCameraFollowTarget);
        EmitSignalChaseStopped();
        _isChasing = false;
    }
}