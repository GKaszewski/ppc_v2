using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class CollectableResource : Resource
{
    [Export] public float Amount { get; set; } = 0.0f;
    [Export] public CollectableType Type { get; set; }
}