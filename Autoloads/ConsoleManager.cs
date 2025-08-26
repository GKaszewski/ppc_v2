using Godot;
using Limbo.Console.Sharp;
using Mr.BrickAdventures.scripts;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.Autoloads;

public partial class ConsoleManager : Node
{
    private GameManager _gameManager;
    private SkillManager _skillManager;
    private SkillUnlockerComponent _skillUnlockerComponent;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");

        RegisterConsoleCommands();
    }

    public override void _ExitTree()
    {
        UnregisterConsoleCommands();
    }

    [ConsoleCommand("add_coins", "Adds a specified amount of coins to the player's total.")]
    private void AddCoinsCommand(int amount)
    {
        _gameManager.AddCoins(amount);
        LimboConsole.Info($"Increased coins by {amount}. Total coins: {_gameManager.GetCoins()}");
    }

    [ConsoleCommand("set_coins", "Sets the player's total coins to a specified amount.")]
    private void SetCoinsCommand(int amount)
    {
        _gameManager.SetCoins(amount);
        LimboConsole.Info($"Set coins to {amount}. Total coins: {_gameManager.GetCoins()}");
    }

    [ConsoleCommand("set_lives", "Sets the player's total lives to a specified amount.")]
    private void SetLivesCommand(int amount)
    {
        _gameManager.SetLives(amount);
        LimboConsole.Info($"Set lives to {amount}.");
    }

    [ConsoleCommand("add_lives", "Adds a specified amount of lives to the player's total.")]
    private void AddLivesCommand(int amount)
    {
        _gameManager.AddLives(amount);
        LimboConsole.Info($"Increased lives by {amount}. Total lives: {_gameManager.GetLives()}");
    }

    [ConsoleCommand("set_health", "Sets the player's health to a specified amount.")]
    private void SetHealthCommand(float amount)
    {
        var playerHealthComponent = _gameManager.Player.GetNode<HealthComponent>("HealthComponent");
        if (playerHealthComponent != null)
        {
            playerHealthComponent.Health = amount;
            LimboConsole.Info($"Set player health to {amount}.");
        }
        else
        {
            LimboConsole.Warn("Player HealthComponent not found.");
        }
    }

    [ConsoleCommand("reset_session", "Resets the current session state.")]
    private void ResetSessionCommand()
    {
        _gameManager.ResetCurrentSessionState();
        LimboConsole.Info("Current session state has been reset.");
    }

    [ConsoleCommand("unlock_skill", "Unlocks and activates a skill by its name.")]
    private void UnlockSkillCommand(string skillName)
    {
        if (!GetSkillManagement()) return;

        var skill = _skillManager.GetSkillByName(skillName);
        if (skill == null)
        {
            LimboConsole.Warn($"Skill '{skillName}' not found.");
            return;
        }

        _gameManager.UnlockSkill(skill);
        _skillManager.ActivateSkill(skill);
        _skillUnlockerComponent.EmitSignal(SkillUnlockerComponent.SignalName.SkillUnlocked, skill);
        LimboConsole.Info($"Skill '{skillName}' has been unlocked and activated.");
    }

    private bool GetSkillManagement()
    {
        var player = _gameManager.Player;
        if (player == null)
        {
            LimboConsole.Warn("Player node not found.");
            return false;
        }

        _skillManager ??= player.GetNode<SkillManager>("SkillManager");
        _skillUnlockerComponent ??= player.GetNode<SkillUnlockerComponent>("SkillUnlockerComponent");

        if (_skillManager != null && _skillUnlockerComponent != null) return true;

        LimboConsole.Warn("SkillManager or SkillUnlockerComponent not found on the player.");
        return false;

    }

    [ConsoleCommand("unlock_all_skills", "Unlocks and activates all available skills.")]
    private void UnlockAllSkillsCommand()
    {
        if (!GetSkillManagement()) return;

        _skillUnlockerComponent.UnlockAllSkills();
        LimboConsole.Info("All skills have been unlocked and activated.");
    }
    
    [ConsoleCommand("remove_skill", "Deactivates and removes a skill by its name.")]
    private void RemoveSkillCommand(string skillName)
    {
        if (!GetSkillManagement()) return;

        var skill = _skillManager.GetSkillByName(skillName);
        if (skill == null)
        {
            LimboConsole.Warn($"Skill '{skillName}' not found.");
            return;
        }

        _gameManager.RemoveSkill(skill.Name);
        _skillManager.DeactivateSkill(skill);
        LimboConsole.Info($"Skill '{skillName}' has been deactivated.");
    }
    
    [ConsoleCommand("remove_all_skills", "Deactivates and removes all skills.")]
    private void RemoveAllSkillsCommand()
    {
        if (!GetSkillManagement()) return;
        
        foreach (var skill in _skillManager.AvailableSkills)
        {
            _gameManager.RemoveSkill(skill.Name);
            _skillManager.DeactivateSkill(skill);
        }
        LimboConsole.Info("All skills have been deactivated.");
    }

}