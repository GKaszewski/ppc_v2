using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class SideToSideMovementComponent : Node
{
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public float Speed { get; set; } = 10.0f;
    [Export] public float WaitTime { get; set; } = 1.0f;
    [Export] public RayCast2D LeftRay { get; set; }
    [Export] public RayCast2D RightRay { get; set; }
    [Export] public RayCast2D LeftWallRay { get; set; }
    [Export] public RayCast2D RightWallRay { get; set; }
    
    private Vector2 _direction = Vector2.Left;
    private Vector2 _newDirection = Vector2.Left;
    private Timer _timer;
    private bool _triggeredDirectionChange = false;

    [Signal]
    public delegate void DirectionChangedEventHandler();
    
    public Vector2 Direction => _direction;

    public override void _Ready()
    {
        SetupTimer();
        DirectionChanged += OnDirectionChanged;
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleDirection();
        HandleSpriteFlip();
        HandleMovement(delta);
    }

    private void HandleDirection()
    {
        // Check if we are colliding with the left wall
        if (LeftWallRay.IsColliding())
        {
            _newDirection = Vector2.Right;
            EmitSignalDirectionChanged();
            return;
        }

        // Check if we are colliding with the right wall
        if (RightWallRay.IsColliding())
        {
            _newDirection = Vector2.Left;
            EmitSignalDirectionChanged();
            return;
        }
        //  We are not colliding with anything, which means we don't have ground to walk on. Stop moving.
        if (!LeftRay.IsColliding() && !RightRay.IsColliding())
        {
            _newDirection = Vector2.Zero;
            return;
        }

        // If the left ray is not colliding and the right ray is colliding, that means we have ground to the right and we should change direction to the right.
        if (!LeftRay.IsColliding() && RightRay.IsColliding())
        {
            _newDirection = Vector2.Right;
            EmitSignalDirectionChanged();
            return;
        }

        // If the right ray is not colliding and the left ray is colliding, that means we have ground to the left and we should change direction to the left.
        if (!RightRay.IsColliding() && LeftRay.IsColliding())
        {
            _newDirection = Vector2.Left;
            EmitSignalDirectionChanged();
            return;
        }
    }

    private void HandleSpriteFlip()
    {
        Sprite.FlipH = _direction == Vector2.Left;
    }

    private void HandleMovement(double delta)
    {
        var root = Owner as Node2D;
        if (root == null) return;
        
        root.Position += _direction * Speed * (float)delta;
    }

    private void OnDirectionChanged()
    {
        if (_direction == _newDirection || _triggeredDirectionChange)
            return;
        
        _triggeredDirectionChange = true;
        _direction = Vector2.Zero;
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        _timer.Stop();
        _direction = _newDirection;
        _triggeredDirectionChange = false;
    }

    private void SetupTimer()
    {
        _timer = new Timer();
        AddChild(_timer);
        _timer.WaitTime = WaitTime;
        _timer.OneShot = true;
        _timer.Timeout += OnTimerTimeout;
    }
}