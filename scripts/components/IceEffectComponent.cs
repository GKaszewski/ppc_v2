using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class IceEffectComponent : Node
{
    [Export] public Array<Node> ComponentsToDisable { get; set; } = [];
    [Export] public StatusEffectComponent StatusEffectComponent { get; set; }
    [Export] public Node2D IceFx { get; set; }

    private StatusEffectDataResource _data = null;
    private int _iceEffectsApplied = 0;

    public override void _Ready()
    {
        StatusEffectComponent.EffectApplied += OnEffectApplied;
        StatusEffectComponent.EffectRemoved += OnEffectRemoved;
    }

    private void OnEffectApplied(StatusEffect statusEffect)
    {
        if (statusEffect.EffectData.Type != StatusEffectType.Ice) return;
        
        _data = statusEffect.EffectData;
        _iceEffectsApplied++;
        ApplyFreeze();
    }
    
    private void OnEffectRemoved(StatusEffectType type)
    {
        if (type != StatusEffectType.Ice) return;
        _data = null;
        _iceEffectsApplied--;
        RemoveFreeze();
    }

    private void ApplyFreeze()
    {
        if (IceFx != null)
        {
            IceFx.Visible = true;
        }

        foreach (var component in ComponentsToDisable)
        {
            if (component == null || _iceEffectsApplied == 0) continue;

            component.ProcessMode = ProcessModeEnum.Disabled;
        }
    }

    private void RemoveFreeze()
    {
        if (_iceEffectsApplied > 0) return;

        if (IceFx != null)
        {
            IceFx.Visible = false;
        }
        
        foreach (var component in ComponentsToDisable)
        {
            if (component == null) continue;

            component.ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}