using System.Linq;
using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class DoubleJumpSkillComponent : Node, ISkill
{
    [Export] private PackedScene _doubleJumpAbilityScene;
    private PlayerController _playerController;
    
    public void Initialize(Node owner, SkillData data)
    {
        _playerController = owner as PlayerController;
        if (_playerController == null)
        {
            GD.PrintErr("DoubleJumpSkillComponent must be a child of a PlayerController.");
        }
    }

    public void Activate()
    {
        if (_playerController == null) return;

        var hasAbility = _playerController.GetActiveAbilities().Any(ability => ability is DoubleJumpAbility);

        if (!hasAbility)
        {
            var abilityInstance = _doubleJumpAbilityScene.Instantiate<DoubleJumpAbility>();
            _playerController.AddAbility(abilityInstance);
        }
    }

    public void Deactivate()
    {
        _playerController?.RemoveAbility<DoubleJumpAbility>();
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
    {
        
    }
}