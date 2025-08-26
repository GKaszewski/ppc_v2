using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class TapThrowInputResource : ThrowInputResource
{
    
    public override void ProcessInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            EmitSignalThrowRequested(1f);
        }
    }

    public override bool SupportsCharging()
    {
        return false;
    }
}