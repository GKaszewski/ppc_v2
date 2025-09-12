using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class GhostMovementComponent : Node2D
{
    [Export] public float MoveSpeed { get; set; } = 0.2f;
    [Export] public int GridSize { get; set; } = 16;
    
    private CharacterBody2D _body;
    private Timer _moveTimer;
    private PacXonGridManager _gridManager;
    private HealthComponent _playerHealth;
    private Vector2 _direction;
    private readonly Vector2[] _directions = { Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };
    
    public override void _Ready()
    {
        _body = Owner.GetNode<CharacterBody2D>(".");
        _moveTimer = new Timer { WaitTime = MoveSpeed, OneShot = false, Autostart = true };
        AddChild(_moveTimer);
        _moveTimer.Timeout += OnMoveTimerTimeout;

        var rng = new RandomNumberGenerator();
        _direction = _directions[rng.RandiRange(0, 3)];
    }
    
    public void Initialize(PacXonGridManager gridManager, PlayerController player)
    {
        _gridManager = gridManager;
        _playerHealth = player.GetNode<HealthComponent>("HealthComponent");
    }
    
    private void OnMoveTimerTimeout()
    {
        if (_gridManager == null || _body == null) return;

        var nextMapPos = _gridManager.LocalToMap(_body.Position + (_direction * GridSize));
        var cellState = _gridManager.GetCellState(nextMapPos);

        switch (cellState)
        {
            case CellState.Solid:
                PickNewDirection();
                break;

            case CellState.Trail:
                _playerHealth?.DecreaseHealth(9999);
                _moveTimer.Stop(); 
                break;

            case CellState.Empty:
                _body.Position += _direction * GridSize;
                break;
        }
    }

    private void PickNewDirection()
    {
        var rng = new RandomNumberGenerator();
        Vector2 newDir;
        do
        {
            newDir = _directions[rng.RandiRange(0, 3)];
        } while (newDir == _direction || newDir == -_direction);
        _direction = newDir;
    }
}