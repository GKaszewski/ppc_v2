using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PlayerInputHandler : Node
{
    public Vector2 MoveDirection { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool JumpHeld { get; private set; }
    
    public override void _Process(double delta)
    {
        MoveDirection = Input.GetVector("left", "right", "up", "down");
        
        JumpPressed = Input.IsActionJustPressed("jump");
        JumpReleased = Input.IsActionJustReleased("jump");
        JumpHeld = Input.IsActionPressed("jump");
    }
}