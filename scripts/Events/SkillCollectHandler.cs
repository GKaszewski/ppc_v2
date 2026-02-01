using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.Events;

/// <summary>
/// Handles skill collection events and unlocks skills via GameStateStore.
/// Skills are immediately activated but only persisted on level complete.
/// </summary>
public partial class SkillCollectHandler : Node
{
    private SkillManager _skillManager;

    public override void _Ready()
    {
        _skillManager = SkillManager.Instance;
        EventBus.Instance.SkillCollected += OnSkillCollected;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.SkillCollected -= OnSkillCollected;
        }
    }

    private void OnSkillCollected(SkillData skill, Vector2 position)
    {
        if (skill == null) return;

        // Unlock in session (will be committed on level complete, lost on death)
        GameStateStore.Instance?.UnlockSkillInSession(skill);

        // Immediately activate the skill for the player
        skill.Level = 1;
        _skillManager?.AddSkill(skill);

        // Emit skill unlocked event for UI/achievements
        EventBus.EmitSkillUnlocked(skill.Name, skill.Level);
    }
}
