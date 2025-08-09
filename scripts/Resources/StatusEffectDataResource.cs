using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class StatusEffectDataResource : Resource
{
    [Export] public StatusEffectType Type { get; set; }
}