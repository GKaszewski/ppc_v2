using System;
using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class SkillData : Resource
{
    [Export] public string Name { get; set; } = "New Skill";
    [Export] public string Description { get; set; } = "New Skill";
    [Export] public Texture2D Icon { get; set; }
    [Export] public bool IsActive { get; set; } = false;
    [Export] public int Level { get; set; } = 1;
    [Export] public SkillType Type { get; set; } = SkillType.Throw;
    [Export] public PackedScene Node { get; set; }
    [Export] public Array<SkillUpgrade> Upgrades { get; set; } = [];
    
    public int MaxLevel => Upgrades.Count;
}