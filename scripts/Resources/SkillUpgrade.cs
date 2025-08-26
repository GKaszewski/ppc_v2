using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.scripts.Resources;

[GlobalClass]
public partial class SkillUpgrade : Resource
{
    [Export] public int Cost { get; set; } = 50;
    [Export(PropertyHint.MultilineText)] public string Description { get; set; } = "Upgrade Description";
    [Export] public Dictionary<string, Variant> Properties { get; set; } = new();
}