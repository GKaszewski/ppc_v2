using Godot;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.Autoloads;

public partial class ConsoleManager : Node
{
    private GameManager _gameManager;
    private SkillManager _skillManager;
    private SkillUnlockerComponent _skillUnlockerComponent;
    private AchievementManager _achievementManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _achievementManager = GetNode<AchievementManager>("/root/AchievementManager");
        _skillManager = GetNode<SkillManager>("/root/SkillManager");
    }

    private void AddCoinsCommand(int amount)
    {
        _gameManager.AddCoins(amount);
    }

    private void SetCoinsCommand(int amount)
    {
        _gameManager.SetCoins(amount);
    }

    private void SetLivesCommand(int amount)
    {
        _gameManager.SetLives(amount);
    }

    private void AddLivesCommand(int amount)
    {
        _gameManager.AddLives(amount);
    }

    private void SetHealthCommand(float amount)
    {
        var playerHealthComponent = _gameManager.Player.GetNode<HealthComponent>("HealthComponent");
        if (playerHealthComponent != null)
        {
            playerHealthComponent.Health = amount;
        }
    }

    private void ResetSessionCommand()
    {
        _gameManager.ResetCurrentSessionState();
    }

    private void UnlockSkillCommand(string skillName)
    {
        if (!GetSkillManagement()) return;

        var skill = _skillManager.GetSkillByName(skillName);
        if (skill == null)
        {
            return;
        }

        _gameManager.UnlockSkill(skill);
        _skillManager.ActivateSkill(skill);
        _skillUnlockerComponent.EmitSignal(SkillUnlockerComponent.SignalName.SkillUnlocked, skill);
    }

    private bool GetSkillManagement()
    {
        var player = _gameManager.Player;
        if (player == null || !IsInstanceValid(player))
        {
            return false;
        }

        _skillUnlockerComponent ??= player.GetNode<SkillUnlockerComponent>("SkillUnlockerComponent");

        if (_skillManager != null && _skillUnlockerComponent != null) return true;

        return false;

    }

    private void UnlockAllSkillsCommand()
    {
        if (!GetSkillManagement()) return;

        _skillUnlockerComponent.UnlockAllSkills();
    }
    
    private void RemoveSkillCommand(string skillName)
    {
        if (!GetSkillManagement()) return;

        var skill = _skillManager.GetSkillByName(skillName);
        if (skill == null)
        {
            return;
        }

        _gameManager.RemoveSkill(skill.Name);
        _skillManager.DeactivateSkill(skill);
    }
    
    private void RemoveAllSkillsCommand()
    {
        if (!GetSkillManagement()) return;
        
        foreach (var skill in _skillManager.AvailableSkills)
        {
            _gameManager.RemoveSkill(skill.Name);
            _skillManager.DeactivateSkill(skill);
        }
    }
    
    private void GoToNextLevelCommand()
    {
        _gameManager.OnLevelComplete();
    }
    
    private void UnlockAchievementCommand(string achievementId)
    {
        _achievementManager.UnlockAchievement(achievementId);
    }
    
    private void ResetAchievementCommand(string achievementId)
    {
        _achievementManager.LockAchievement(achievementId);
    }

}