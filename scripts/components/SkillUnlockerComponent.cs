using Godot;
using Godot.Collections;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class SkillUnlockerComponent : Node
{
    public SkillManager SkillManager { get; private set; }

    [Signal]
    public delegate void SkillUnlockedEventHandler(SkillData skill);

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GameManager.Instance;
        SkillManager = SkillManager.Instance;
    }

    private bool HasEnoughCoins(int amount)
    {
        return _gameManager != null && _gameManager.GetCoins() >= amount;
    }

    public bool TryUnlockSkill(SkillData skill)
    {
        if (_gameManager == null) return false;
        if (_gameManager.IsSkillUnlocked(skill)) return false;
        if (!HasEnoughCoins(skill.Upgrades[0].Cost)) return false;

        skill.Level = 1;
        _gameManager.RemoveCoins(skill.Upgrades[0].Cost);

        // Add to session state via GameStateStore
        GameStateStore.Instance?.UnlockSkillInSession(skill);
        SkillManager.AddSkill(skill);
        EmitSignalSkillUnlocked(skill);

        return true;
    }

    public void UnlockAllSkills()
    {
        var availableSkills = SkillManager.AvailableSkills;

        foreach (var skill in availableSkills)
        {
            EmitSignalSkillUnlocked(skill);
        }

        _gameManager.UnlockSkills(availableSkills);
        SkillManager.ApplyUnlockedSkills();
    }

    public bool TryUpgradeSkill(SkillData skill)
    {
        if (_gameManager == null) return false;
        if (!_gameManager.IsSkillUnlocked(skill)) return false;
        if (skill.Level >= skill.MaxLevel) return false;
        if (!HasEnoughCoins(skill.Upgrades[skill.Level].Cost)) return false;

        _gameManager.RemoveCoins(skill.Upgrades[skill.Level].Cost);
        skill.Level++;
        if (SkillManager.ActiveComponents.TryGetValue(skill.Name, out Variant componentVariant))
        {
            var component = componentVariant.AsGodotObject();
            if (component is ISkill skillInstance)
            {
                skillInstance.ApplyUpgrade(skill.Upgrades[skill.Level - 1]);
            }
        }
        EmitSignalSkillUnlocked(skill);
        return true;
    }
}