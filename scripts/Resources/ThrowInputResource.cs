using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.Resources;

public abstract partial class ThrowInputResource : Resource, IThrowInput
{
    [Signal] public delegate void ThrowRequestedEventHandler(float powerMultiplier = 1f);

    public virtual void ProcessInput(InputEvent @event)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update(double delta)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool SupportsCharging()
    {
        throw new System.NotImplementedException();
    }
}