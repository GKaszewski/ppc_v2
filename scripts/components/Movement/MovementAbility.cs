using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public abstract partial class MovementAbility : Node
{
    protected PlayerController _controller;
    protected CharacterBody2D _body;
    protected PlayerInputHandler _input;
    
    public virtual void Initialize(PlayerController controller)
    {
        Name = $"{this.GetType().Name}";
        
        _controller = controller;
        if (_controller == null)
        {
            GD.PushError($"Movement ability '{Name}' must be a child of a PlayerController.");
            SetProcess(false);
            SetPhysicsProcess(false);
            return;
        }

        _body = _controller;
        _input = _controller.GetNode<PlayerInputHandler>("PlayerInputHandler");
        if (_input == null)
        {
            GD.PushError($"PlayerController '{_controller.Name}' must have a PlayerInputHandler child.");
            SetProcess(false);
            SetPhysicsProcess(false);
        }
        
        _body.Velocity = Vector2.Zero;
    }

    public abstract Vector2 ProcessMovement(Vector2 currentVelocity, double delta);
}