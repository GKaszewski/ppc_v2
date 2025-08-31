using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class CleanupComponent : Node
{
    public void CleanUp()
    {
        Owner.QueueFree();
    }
}