using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class JumpAbility : MovementAbility
{
    [Export] public float JumpVelocity { get; set; } = -400.0f;
    [Export] public int CoyoteFrames { get; set; } = 6;
    
    private Timer _coyoteTimer;
    private bool _wasOnFloor = false; 
    
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        _coyoteTimer = new Timer { OneShot = true, WaitTime = CoyoteFrames / (float)Engine.GetPhysicsTicksPerSecond() };
        AddChild(_coyoteTimer);
    }
    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (!_body.IsOnFloor() && _wasOnFloor)
        {
            _coyoteTimer.Start();
        }
        
        if (_input.JumpHeld)
        {
            if (_body.IsOnFloor() || !_coyoteTimer.IsStopped())
            {
                velocity.Y = JumpVelocity;
                _controller.EmitSignal(PlayerController.SignalName.JumpInitiated);
                _coyoteTimer.Stop();
            }
        }
        
        _wasOnFloor = _body.IsOnFloor();
        return velocity;
    }
}