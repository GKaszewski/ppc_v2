using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.app;

public sealed class AppSkillCatalog : ISkillCatalog
{
    private readonly Dictionary<string, SkillDef> _all;

    public AppSkillCatalog()
    {
        _all = new Dictionary<string, SkillDef>
        {
            ["brick_throw"] = new SkillDef("brick_throw",
                "BRICK_THROW",
                SkillType.Throw,
                1,
                50,
                "res://objects/player_skills/brick_throw_skill.tscn",
                new Godot.Collections.Dictionary<string, Variant>
                {
                    { "FireRate", 0.30f },
                    { "BrickScene", "res://scenes/projectiles/brick_bullet.tscn" }
                }
                ),
            ["fire_brick_throw"] = new SkillDef("fire_brick_throw", "FIRE_BRICK", SkillType.Throw, 3, 100,
                "res://scenes/skills/throw_brick_fire.tscn"),
            ["ice_brick_throw"] = new SkillDef("ice_brick_throw", "ICE_BRICK", SkillType.Throw, 3, 100,
                "res://scenes/skills/throw_brick_ice.tscn"),
            ["explosive_brick_throw"] = new SkillDef("explosive_brick_throw", "EXPLOSIVE_BRICK", SkillType.Throw, 3, 150,
                "res://scenes/skills/throw_brick_explosive.tscn"),
            ["magnetic"] = new SkillDef("magnetic", "MAGNETIC", SkillType.Misc, 2, 120,
                "res://scenes/skills/magnetic.tscn")
        };
    }
    
    public IReadOnlyDictionary<string, SkillDef> All => _all;
    
    public bool TryGet(string id, out SkillDef def) => _all.TryGetValue(id, out def);
}