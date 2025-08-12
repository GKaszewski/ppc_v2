using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class CleanupComponent : Node
{
    public void CleanUp()
    {
        Owner.QueueFree();
    }
}