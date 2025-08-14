using Godot;

namespace Mr.BrickAdventures.scripts.interfaces;

public interface IThrowInput
{
    public void ProcessInput(InputEvent @event);
    public void Update(double delta);
    public bool SupportsCharging();
}