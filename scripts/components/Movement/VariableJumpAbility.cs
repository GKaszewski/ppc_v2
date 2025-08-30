using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class VariableJumpAbility : MovementAbility
{
    [ExportGroup("Jump Design")]
    [Export] public float JumpHeight { get; set; } = 100f;
    [Export] public float JumpTimeToPeak { get; set; } = 0.5f;
    [Export] public float JumpTimeToDescent { get; set; } = 0.4f;
    
    [ExportGroup("Jump Feel")]
    // How much to reduce upward velocity when the jump button is released mid-air.
    [Export(PropertyHint.Range, "0.0, 1.0, 0.05")] public float JumpCutMultiplier { get; set; } = 0.5f;
    [Export(PropertyHint.Range, "0,10,1")] public int CoyoteFrames { get; set; } = 6;
    
    private float _jumpVelocity;
    private bool _wasOnFloor = false;
    private bool _hasJumpedInAir = false;
    private Timer _coyoteTimer;
    
    public float AscendGravity { get; private set; }
    public float DescendGravity { get; private set; }
    
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        
        _jumpVelocity = (2.0f * JumpHeight) / JumpTimeToPeak * -1.0f;
        AscendGravity = (-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak) * -1.0f;
        DescendGravity = (-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent) * -1.0f;

        _coyoteTimer = new Timer { OneShot = true, WaitTime = CoyoteFrames / (float)Engine.GetPhysicsTicksPerSecond() };
        AddChild(_coyoteTimer);
    }

    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        var isGrounded = _body.IsOnFloor();

        if (!isGrounded && _wasOnFloor) _coyoteTimer.Start();
        if (isGrounded) _hasJumpedInAir = false;

        if (_input.JumpHeld && !_hasJumpedInAir)
        {
            if (isGrounded || !_coyoteTimer.IsStopped())
            {
                velocity.Y = _jumpVelocity;
                _controller.EmitSignal(PlayerController.SignalName.JumpInitiated);
                _coyoteTimer.Stop();
                _hasJumpedInAir = true;
            }
        }
        
        if (_input.JumpReleased)
        {
            if (velocity.Y < 0.0f)
            {
                velocity.Y *= JumpCutMultiplier;
            }
        }

        _wasOnFloor = isGrounded;
        return velocity;
    }
}