using Godot;

namespace Mr.BrickAdventures.scripts.components;

public abstract partial class MovementAbility : Node
{
    protected PlayerController _controller;
    protected CharacterBody2D _body;
    protected PlayerInputHandler _input;

    public override void _Ready()
    {
        CallDeferred(nameof(Initialize));
    }

    public virtual void Initialize()
    {
        _controller = GetOwner<PlayerController>();
        if (_controller == null)
        {
            GD.PushError($"Movement ability '{Name}' must be a child of a PlayerController.");
            SetProcess(false);
            SetPhysicsProcess(false);
            return;
        }

        _body = _controller;
        _input = _controller.GetNode<PlayerInputHandler>("PlayerInputHandler");
        GD.Print($"Input handler for '{Name}' set to '{_input.Name}'");
    }

    public abstract Vector2 ProcessMovement(Vector2 currentVelocity, double delta);
}