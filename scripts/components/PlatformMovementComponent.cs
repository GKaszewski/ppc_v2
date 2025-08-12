using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class PlatformMovementComponent : Node2D, IMovement
{
    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float JumpHeight { get; set; } = 100f;

    [Export]
    public float JumpTimeToPeak { get; set; } = 0.5f;

    [Export]
    public float JumpTimeToDescent { get; set; } = 0.4f;

    [Export]
    public int CoyoteFrames { get; set; } = 6;

    [Export]
    public AudioStreamPlayer2D JumpSfx { get; set; }

    [Export]
    public Node2D RotationTarget { get; set; }

    [Export]
    public CharacterBody2D Body { get; set; }

    private float _gravity;
    private bool _wasLastFloor = false;
    private bool _coyoteMode = false;
    private Timer _coyoteTimer;
    private Vector2 _lastDirection = new Vector2(1, 0);

    private float _jumpVelocity;
    private float _jumpGravity;
    private float _fallGravity;
    
    public Vector2 LastDirection => _lastDirection;
    
    public override void _Ready()
    {
        base._Ready();

        if (Body == null)
            return;

        _gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
        _jumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
        _jumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
        _fallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;

        _coyoteTimer = new Timer
        {
            OneShot = true,
            WaitTime = CoyoteFrames / 60.0f
        };
        _coyoteTimer.Timeout += OnCoyoteTimerTimeout;
        AddChild(_coyoteTimer);
    }

    public string MovementType { get; } = "platform";
    public bool Enabled { get; set; }
    public Vector2 PreviousVelocity { get; set; }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Body == null || !Enabled)
            return;

        if (Body.Velocity.X > 0.0f)
            RotationTarget.Rotation = Mathf.DegToRad(-10);
        else if (Body.Velocity.X < 0.0f)
            RotationTarget.Rotation = Mathf.DegToRad(10);
        else
            RotationTarget.Rotation = 0;

        CalculateJumpVars();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Body == null || !Enabled)
            return;

        if (Body.IsOnFloor())
        {
            _wasLastFloor = true;
            _coyoteMode = false;  // Reset coyote mode when back on the floor
            _coyoteTimer.Stop();  // Stop timer when grounded
        }
        else
        {
            if (_wasLastFloor) // Start coyote timer only once
            {
                _coyoteMode = true;
                _coyoteTimer.Start();
            }
            _wasLastFloor = false;
        }

        if (!Body.IsOnFloor())
            Body.Velocity += new Vector2(0, CalculateGravity()) * (float)delta;

        if (Input.IsActionPressed("jump") && (Body.IsOnFloor() || _coyoteMode))
            Jump();

        if (Input.IsActionJustPressed("down"))
            Body.Position += new Vector2(0, 1);

        float direction = Input.GetAxis("left", "right");
        if (direction != 0)
            _lastDirection = HandleDirection(direction);

        if (direction != 0)
            Body.Velocity = new Vector2(direction * Speed, Body.Velocity.Y);
        else
            Body.Velocity = new Vector2(Mathf.MoveToward(Body.Velocity.X, 0, Speed), Body.Velocity.Y);

        Body.MoveAndSlide();
    }

    private void Jump()
    {
        if (Body == null)
            return;

        Body.Velocity = new Vector2(Body.Velocity.X, _jumpVelocity);
        _coyoteMode = false;
        if (JumpSfx != null)
            JumpSfx.Play();
    }

    private float CalculateGravity()
    {
        return Body.Velocity.Y < 0.0f ? _jumpGravity : _fallGravity;
    }

    private void OnCoyoteTimerTimeout()
    {
        _coyoteMode = false;
    }

    private Vector2 HandleDirection(float inputDir)
    {
        if (inputDir > 0)
            return new Vector2(1, 0);
        else if (inputDir < 0)
            return new Vector2(-1, 0);
        return _lastDirection;
    }

    public void OnShipEntered()
    {
        RotationTarget.Rotation = 0;
    }

    private void CalculateJumpVars()
    {
        _jumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
        _jumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
        _fallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;
    }
}