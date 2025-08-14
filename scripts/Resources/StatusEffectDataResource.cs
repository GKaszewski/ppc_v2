using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class StatusEffectDataResource : Resource
{
    [Export] public float Duration { get; set; } = 1f;
    [Export] public float DamagePerSecond { get; set; } = 0.25f;
    [Export] public StatusEffectType Type { get; set; }
}