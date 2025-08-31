using Godot;
using PhantomCamera;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class ChaseLevelComponent : Node
{
    [Export] public float ChaseSpeed { get; set; } = 200.0f;
    [Export] public Marker2D ChaseTarget { get; set; }
    [Export] public float MinimumDistance { get; set; } = 10f;

    [Signal]
    public delegate void ChaseStartedEventHandler();

    [Signal]
    public delegate void ChaseStoppedEventHandler();

    private bool _isChasing = false;
    private Node2D _previousCameraFollowTarget = null;
    private PhantomCamera2D _phantomCamera = null;
    private Node2D _root = null;
    

    public override void _Ready()
    {
        _phantomCamera = GetNode<Node2D>("../../%PhantomCamera").AsPhantomCamera2D();
        _root = Owner as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_isChasing) return;
        if (ChaseTarget == null) return;

        if (CheckIfReachedTarget())
        {
            StopChasing();
            return;
        }
        
        var targetPosition = ChaseTarget.GlobalPosition;
        
        if (_root == null) return;
        
        var direction = (targetPosition - _root.GlobalPosition).Normalized();
        var speed = direction * ChaseSpeed * (float)delta;
        _root.GlobalPosition += speed;
    }

    public void OnShipEntered()
    {
        if (ChaseTarget == null || _phantomCamera == null)
            return;

        if (_isChasing) return;

        _previousCameraFollowTarget = _phantomCamera.FollowTarget;
        _phantomCamera.FollowTarget = _root;
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
        if (_phantomCamera == null) return;

        _phantomCamera.FollowTarget = _previousCameraFollowTarget;
        EmitSignalChaseStopped();
        _isChasing = false;
    }
    
    public void SetChasing(bool shouldChase)
    {
        if (shouldChase && !_isChasing)
        {
            _previousCameraFollowTarget = _phantomCamera.FollowTarget;
            _phantomCamera.FollowTarget = _root;
            EmitSignalChaseStarted();
            _isChasing = true;
        }
        else if (!shouldChase && _isChasing)
        {
            StopChasing();
        }
    }
}