using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.Autoloads;

public partial class SkillManager : Node
{
    private GameManager _gameManager;
    private PlayerController _player;

    [Export] public Array<SkillData> AvailableSkills { get; set; } = [];
    
    public Dictionary ActiveComponents { get; private set; } = new();

    [Signal]
    public delegate void ActiveThrowSkillChangedEventHandler(BrickThrowComponent throwComponent);
    [Signal]
    public delegate void SkillRemovedEventHandler(SkillData skillData);
    
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }
    
    /// <summary>
    /// Called by the PlayerController from its _Ready method to register itself with the manager.
    /// </summary>
    public void RegisterPlayer(PlayerController player)
    {
        if (_player == player) return;

        // If a player is already registered (e.g., from a previous scene), unregister it first.
        if (_player != null && IsInstanceValid(_player))
        {
            UnregisterPlayer();
        }
        
        _player = player;
        if (_player != null)
        {
            // Automatically unregister when the player node is removed from the scene.
            _player.TreeExiting += UnregisterPlayer;
            ApplyUnlockedSkills();
        }
    }

    /// <summary>
    /// Cleans up skills and references related to the current player.
    /// </summary>
    private void UnregisterPlayer()
    {
        if (_player != null && IsInstanceValid(_player))
        {
            _player.TreeExiting -= UnregisterPlayer;
            RemoveAllActiveSkills();
        }
        _player = null;
    }
    
    public void AddSkill(SkillData skillData)
    {
        // Ensure a valid player is registered before adding a skill.
        if (_player == null || !IsInstanceValid(_player))
        {
            GD.Print("SkillManager: Player not available to add skill.");
            return;
        }
    
        if (ActiveComponents.ContainsKey(skillData.Name))
            return;

        if (skillData.Type == SkillType.Throw)
        {
            var unlocked = _gameManager.GetUnlockedSkills();
            foreach (var sd in unlocked)
            {
                SkillData data = null;
                foreach (var s in AvailableSkills)
                {
                    if (s == sd)
                    {
                        data = s;
                        break;
                    }
                }
                // Remove other throw skills if a new one is added
                if (data is { Type: SkillType.Throw })
                    RemoveSkill(data.Name);
            }
        }

        var instance = skillData.Node.Instantiate();
        if (instance is ISkill skill)
        {
            // Initialize the skill with the registered player instance.
            skill.Initialize(_player, skillData); 
            skill.Activate();
        } 
        else
        {
            GD.PrintErr($"Skill scene for '{skillData.Name}' does not implement ISkill!");
            instance.QueueFree();
            return;
        }

        // Add the skill node as a child of the player.
        _player.AddChild(instance);
        ActiveComponents[skillData.Name] = instance;
        
        if (instance is BrickThrowComponent btc)
        {
           EmitSignalActiveThrowSkillChanged(btc);
        }
    }
    
    public void RemoveSkill(string skillName)
    {
        if (!ActiveComponents.TryGetValue(skillName, out var component))
            return;
        
        if (component.AsGodotObject() is BrickThrowComponent)
        {
            EmitSignalActiveThrowSkillChanged(null);
        }

        var inst = (Node)component;
        if (inst is ISkill skill)
        {
            skill.Deactivate();
        }
        
        if (IsInstanceValid(inst))
            inst.QueueFree();

        var skills = _gameManager.GetUnlockedSkills();
        foreach (var s in skills)
        {
            if (s.Name == skillName)
            {
                s.IsActive = false;
                break;
            }
        }
        ActiveComponents.Remove(skillName);
        var sd = GetSkillByName(skillName);
        if (sd != null) EmitSignalSkillRemoved(sd);
    }
    
    private void RemoveAllActiveSkills()
    {
        // Create a copy of keys to avoid modification during iteration
        var keys = ActiveComponents.Keys.ToArray();
        var skillNames = keys.Select(key => key.ToString()).ToList();

        foreach (var skillName in skillNames)
        {
            RemoveSkill(skillName);
        }
    }

    public void ApplyUnlockedSkills()
    {
        if (_player == null || !IsInstanceValid(_player)) return;

        foreach (var sd in AvailableSkills)
        {
            if (_gameManager.IsSkillUnlocked(sd))
            {
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