using System.Linq;
using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class DoubleJumpSkillComponent : SkillComponentBase
{
    [Export] private PackedScene _doubleJumpAbilityScene;

    public override void Activate()
    {
        if (Player == null) return;

        var hasAbility = Player.GetActiveAbilities().Any(ability => ability is DoubleJumpAbility);

        if (!hasAbility)
        {
            var abilityInstance = _doubleJumpAbilityScene.Instantiate<DoubleJumpAbility>();
            Player.AddAbility(abilityInstance);
        }
    }

    public override void Deactivate()
    {
        Player?.RemoveAbility<DoubleJumpAbility>();
    }
}