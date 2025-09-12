using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GridMovementAbility : MovementAbility
{
    [Export] public float MoveSpeed { get; set; } = 0.15f; // Time in seconds between moves
    [Export] public int GridSize { get; set; } = 16; // Size of one grid cell in pixels

    private Vector2 _currentDirection = Vector2.Zero;
    private Vector2 _nextDirection = Vector2.Zero;
    private Timer _moveTimer;
    
    [Signal]
    public delegate void MovedEventHandler(Vector2 newPosition);
    
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        _moveTimer = new Timer { WaitTime = MoveSpeed, OneShot = false };
        AddChild(_moveTimer);
        _moveTimer.Timeout += OnMoveTimerTimeout;
        _moveTimer.Start();
    }
    
    public override Vector2 ProcessMovement(Vector2 currentVelocity, double delta)
    {
        GD.Print($"Player position: {_body.Position}, {_body.GlobalPosition}");
        
        var inputDirection = _input.MoveDirection;
        var newDirection = Vector2.Zero;
        
        if (Mathf.Abs(inputDirection.Y) > 0.1f)
        {
            newDirection = new Vector2(0, Mathf.Sign(inputDirection.Y));
        }
        else if (Mathf.Abs(inputDirection.X) > 0.1f)
        {
            newDirection = new Vector2(Mathf.Sign(inputDirection.X), 0);
        }
    
        if (newDirection != Vector2.Zero && newDirection != -_currentDirection)
        {
            _nextDirection = newDirection;
        }
        
        return Vector2.Zero; 
    }
    
    private void OnMoveTimerTimeout()
    {
        _currentDirection = _nextDirection;
        if (_currentDirection == Vector2.Zero) return;

        _body.Position += _currentDirection * GridSize;
        EmitSignal(SignalName.Moved, _body.GlobalPosition);
    }
}