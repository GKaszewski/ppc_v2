using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class DoubleJumpAbility : MovementAbility
{
    private bool _hasDoubleJumped = false;
    private float _jumpVelocity;
    
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        
        foreach (var ability in _controller.GetActiveAbilities())
        {
            if (ability is VariableJumpAbility jumpAbility)
            {
                _jumpVelocity = (2.0f * jumpAbility.JumpHeight) / jumpAbility.JumpTimeToPeak * -1.0f;
                break;
            }
        }
        
        if (_jumpVelocity == 0)
        {
            _jumpVelocity = -400.0f; 
        }
    }
    
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (_body.IsOnFloor())
        {
            _hasDoubleJumped = false;
        }

        if (_input.JumpPressed && !_body.IsOnFloor() && !_hasDoubleJumped)
        {
            velocity.Y = _jumpVelocity;
            _controller.EmitSignal(PlayerController.SignalName.JumpInitiated);
            _hasDoubleJumped = true;
        }

        return velocity;
    }
}