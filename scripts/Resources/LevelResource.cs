using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

[GlobalClass]
public partial class LevelResource : Resource
{
    [Export] 
    public string LevelName { get; set; } = string.Empty;

    [Export] 
    public string ScenePath { get; set; } = string.Empty;
}