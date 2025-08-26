using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.interfaces;

public interface ISkill
{
    void Initialize(Node owner, SkillData data);
    
    void Activate();
    void Deactivate();
    void ApplyUpgrade(SkillUpgrade upgrade);
}