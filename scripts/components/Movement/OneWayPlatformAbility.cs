using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class OneWayPlatformAbility : MovementAbility
{
    public override Vector2 ProcessMovement(Vector2 velocity, double delta)
    {
        if (_input.DownHeld && _controller != null)
            _controller.Position += new Vector2(0, 1);
        
        return velocity;
    }
}