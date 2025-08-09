using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class CollectableResource : Resource
{
    [Export] public Variant Amount { get; set; } = 0.0;
    [Export] public CollectableType Type { get; set; }
}