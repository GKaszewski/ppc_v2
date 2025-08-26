using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.Resources;

public abstract partial class ThrowInputResource : Resource, IThrowInput
{
    [Signal] public delegate void ThrowRequestedEventHandler(float powerMultiplier = 1f);

    public virtual void ProcessInput(InputEvent @event)
    {
    }

    public virtual void Update(double delta)
    {
    }

    public virtual bool SupportsCharging()
    {
        return false;
    }
}