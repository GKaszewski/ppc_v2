using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class StatusEffect : GodotObject
{
    public StatusEffectDataResource EffectData { get; set; }
    public float ElapsedTime { get; set; }
    public Timer Timer { get; set; }
}

public partial class StatusEffectComponent : Node
{
    private List<StatusEffect> _activeEffects = [];
    
    [Signal] public delegate void EffectAppliedEventHandler(StatusEffect statusEffect);
    [Signal] public delegate void EffectRemovedEventHandler(StatusEffectType type);

    public void ApplyEffect(StatusEffectDataResource effectData)
    {
        var data = effectData.Duplicate() as StatusEffectDataResource;
        var timer = CreateTimer(effectData.Duration, data);
        
        var statusEffect = new StatusEffect
        {
            EffectData = data,
            ElapsedTime = 0f,
            Timer = timer
        };
        _activeEffects.Add(statusEffect);
        EmitSignalEffectApplied(statusEffect);
    }

    public void RemoveEffect(StatusEffectType type)
    {
        var effectToRemove = _activeEffects.Find(effect => effect.EffectData.Type == type);
        if (effectToRemove.EffectData == null) return;
        _activeEffects.Remove(effectToRemove);
        effectToRemove.Timer.QueueFree();
        EmitSignalEffectRemoved(type);
    }

    private Timer CreateTimer(float duration, StatusEffectDataResource effectData)
    {
        var timer = new Timer();
        timer.SetWaitTime(duration);
        timer.SetOneShot(true);
        timer.SetAutostart(true);
        timer.Timeout += () => RemoveEffect(effectData.Type);
        AddChild(timer);
        return timer;
    }
}