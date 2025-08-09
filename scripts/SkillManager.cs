using Godot;
using Godot.Collections;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts;

public partial class SkillManager : Node
{
    private GameManager _gameManager;
    [Export] public Array<SkillData> AvailableSkills { get; set; } = [];

    public Dictionary ActiveComponents { get; private set; } = new();

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        ApplyUnlockedSkills();
    }

    public void AddSkill(SkillData skillData)
    {
        if (ActiveComponents.ContainsKey(skillData.Name))
            return;

        if (skillData.Type == SkillType.Throw)
        {
            var unlocked = _gameManager.GetUnlockedSkills();
            foreach (var skill in unlocked)
            {
                SkillData data = null;
                foreach (var s in AvailableSkills)
                {
                    if (s == (SkillData)skill)
                    {
                        data = s;
                        break;
                    }
                }
                if (data != null && data.Type == SkillType.Throw)
                    RemoveSkill(data.Name);
            }
        }

        var instance = skillData.Node.Instantiate();
        foreach (var key in skillData.Config.Keys)
        {
            if (instance.HasMethod("get")) // rough presence check
            {
                var value = skillData.Config[key];
                var parent = GetParent();

                if (value.VariantType == Variant.Type.NodePath)
                {
                    var np = (NodePath)value;
                    if (parent.HasNode(np))
                        value = parent.GetNode(np);
                    else if (instance.HasNode(np))
                        value = instance.GetNode(np);
                    else
                        continue;
                }

                // Set via property if exists
                instance.Set(key, value);
            }
        }

        Owner.AddChild(instance);
        ActiveComponents[skillData.Name] = instance;
    }
    
    public void RemoveSkill(string skillName)
    {
        if (!ActiveComponents.TryGetValue(skillName, out var component))
            return;

        var inst = (Node)component;
        if (IsInstanceValid(inst))
            inst.QueueFree();

        var skills = _gameManager.GetUnlockedSkills();
        foreach (SkillData s in skills)
        {
            if (s.Name == skillName)
            {
                s.IsActive = false;
                break;
            }
        }
        ActiveComponents.Remove(skillName);
    }

    public void ApplyUnlockedSkills()
    {
        foreach (var sd in AvailableSkills)
        {
            if (_gameManager.IsSkillUnlocked(sd))
            {
                GD.Print("Applying skill: ", sd.Name);
                CallDeferred(MethodName.AddSkill, sd);
            }
            else
            {
                RemoveSkill(sd.Name);
            }
        }
    }

    public SkillData GetSkillByName(string skillName)
    {
        foreach (var sd in AvailableSkills)
            if (sd.Name == skillName) return sd;
        return null;
    }

    public void ActivateSkill(SkillData skill)
    {
        if (!ActiveComponents.ContainsKey(skill.Name))
        {
            AddSkill(skill);
            skill.IsActive = true;
        }
    }

    public void DeactivateSkill(SkillData skill)
    {
        if (ActiveComponents.ContainsKey(skill.Name))
        {
            RemoveSkill(skill.Name);
            skill.IsActive = false;
        }
    }

    public void ToggleSkillActivation(SkillData skill)
    {
        if (skill == null) return;

        if (ActiveComponents.ContainsKey(skill.Name))
            DeactivateSkill(skill);
        else
            ActivateSkill(skill);
    }
}