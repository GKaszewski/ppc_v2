using Godot;
using Godot.Collections;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class SkillUnlockedComponent : Node
{
    [Export] public SkillManager SkillManager { get; set; }

    [Signal]
    public delegate void SkillUnlockedEventHandler(SkillData skill);

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    private bool HasEnoughCoins(int amount)
    {
        return _gameManager != null && _gameManager.GetCoins() >= amount;
    }

    public bool TryUnlockSkill(SkillData skill)
    {
        if (_gameManager == null) return false;
        if (_gameManager.IsSkillUnlocked(skill)) return false;
        if (!HasEnoughCoins(skill.Cost)) return false;

        skill.Level = 1;
        skill.IsActive = true;
        _gameManager.RemoveCoins(skill.Cost);

        var skillsUnlocked = (Array<SkillData>)_gameManager.CurrentSessionState["skills_unlocked"];
        skillsUnlocked.Add(skill);
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
        if (!HasEnoughCoins(skill.Cost)) return false;
        if (skill.Level >= skill.MaxLevel) return false;

        _gameManager.RemoveCoins(skill.Cost);
        skill.Level++;
        EmitSignalSkillUnlocked(skill);
        return true;
    }
}