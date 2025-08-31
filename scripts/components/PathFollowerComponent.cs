using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PathFollowerComponent : Node2D
{
    [Export] public float Speed { get; set; } = 200f;
    [Export] public bool ShouldRotate { get; set; } = true;

    private PathFollow2D _pathFollowNode;
    private HealthComponent _healthComponent;
    private bool _isActive = false;

    [Signal]
    public delegate void PathCompletedEventHandler();
    [Signal]
    public delegate void EnemyDestroyedEventHandler();
    
    public override void _Ready()
    {
        _healthComponent = GetOwner().GetNodeOrNull<HealthComponent>("HealthComponent");
        if (_healthComponent != null)
        {
            _healthComponent.Death += OnDeath;
        }
    }
    
    public override void _Process(double delta)
    {
        if (!_isActive || _pathFollowNode == null) return;
        
        _pathFollowNode.Progress += Speed * (float)delta;
        
        if (_pathFollowNode.ProgressRatio >= 1.0f)
        {
            _isActive = false;
            EmitSignalPathCompleted();
            _pathFollowNode.QueueFree();
        }
    }

    public void Initialize(PathFollow2D pathFollowNode)
    {
        _pathFollowNode = pathFollowNode;
        if (ShouldRotate)
        {
            _pathFollowNode.Rotates = true;
        }
        else
        {
            _pathFollowNode.Rotates = false;
        }

        _pathFollowNode.Loop = false;
    }

    public void StartFollowing()
    {
        _isActive = true;
    }

    private void OnDeath()
    {
        EmitSignalEnemyDestroyed();
    }
}