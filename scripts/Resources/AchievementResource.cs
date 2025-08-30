using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

[GlobalClass]
public partial class AchievementResource : Resource
{
    [Export] public string Id { get; set; } = ""; // e.g., "level_1_complete"
    [Export] public string DisplayName { get; set; } = "New Achievement";
    [Export(PropertyHint.MultilineText)] public string Description { get; set; } = "";
    [Export] public Texture2D Icon { get; set; }
    [Export] public bool IsSecret { get; set; } = false;
}