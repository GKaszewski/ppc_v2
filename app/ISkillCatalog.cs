using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.app;

public record SkillDef(
    string Id,
    string DisplayName,
    SkillType Type,
    int MaxLevel,
    int BaseCost,
    string EffectScene,
    Godot.Collections.Dictionary<string, Variant>? Config = null
);

public interface ISkillCatalog
{
    IReadOnlyDictionary<string, SkillDef> All { get; }
    bool TryGet(string id, out SkillDef def);
}