using System;
using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class SkillData : Resource
{
    [Export] public String Name { get; set; } = "New Skill";
    [Export] public String Description { get; set; } = "New Skill";
    [Export] public Dictionary<String, Variant> Config { get; set; } = new();
    [Export] public int Cost { get; set; } = 0;
    [Export] public Texture2D Icon { get; set; }
    [Export] public bool IsActive { get; set; } = false;
    [Export] public int Level { get; set; } = 1;
    [Export] public int MaxLevel { get; set; } = 1;
    [Export] public SkillType Type { get; set; } = SkillType.Throw;
    [Export] public PackedScene Node { get; set; }
}