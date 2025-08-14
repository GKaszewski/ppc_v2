using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class TapThrowInputResource : ThrowInputResource
{
    public override void Update(double delta)
    {
        if (Input.IsActionPressed("attack"))
        {
            EmitSignalThrowRequested(1f);
        }
    }

    public override bool SupportsCharging()
    {
        return false;
    }
}