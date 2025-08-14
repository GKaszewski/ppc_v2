using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class EffectInflictorComponent : Node
{
    [Export] public DamageComponent Damage { get; set; }

    public override void _Ready()
    {
        if (Damage == null)
        {
            GD.PushError("EffectInflictorComponent requires a DamageComponent to be set.");
            return;
        }
        
        Damage.EffectInflicted += OnEffectInflicted;
    }

    private void OnEffectInflicted(Node2D target, StatusEffectDataResource effect)
    {
        var statusEffect = target.GetNodeOrNull<StatusEffectComponent>("StatusEffectComponent");

        statusEffect?.ApplyEffect(effect);
    }
}