using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class DamageComponent : Node
{
    [Export] public float Damage { get; set; } = 0.25f;
    [Export] public Area2D Area { get; set; }
    [Export] public StatusEffectDataResource StatusEffectData { get; set; }
    [Export] public Timer DamageTimer { get; set; }
    
    private Node _currentTarget = null;
    
    [Signal] public delegate void EffectInflictedEventHandler(Node2D target, StatusEffectDataResource effect);

    public override void _Ready()
    {
        if (Area == null)
        {
            GD.PushError($"DamageComponent: Area2D node is not set.");
            return;
        }
        
        Area.BodyEntered += OnAreaBodyEntered;
        Area.BodyExited += OnAreaBodyExited;
        Area.AreaEntered += OnAreaAreaEntered;

        if (DamageTimer != null)
        {
            DamageTimer.Timeout += OnDamageTimerTimeout;
        }
    }

    public override void _Process(double delta)
    {
        if (_currentTarget == null) return;
        if (DamageTimer != null) return;
        
        ProcessEntityAndApplyDamage(_currentTarget as Node2D);
    }

    public void DealDamage(HealthComponent target) => target.DecreaseHealth(Damage);

    private void OnAreaAreaEntered(Area2D area)
    {
        if (!CheckIfProcessingIsOn())
            return;
        if (area == Area) return;

        var parent = area.GetParent();
        if (parent.HasNode("DamageComponent"))
            ProcessEntityAndApplyDamage(parent as Node2D);
    }

    private void OnAreaBodyExited(Node2D body)
    {
        if (body != _currentTarget) return;
        _currentTarget = null;
        DamageTimer?.Stop();
    }

    private void OnAreaBodyEntered(Node2D body)
    {
        _currentTarget = body;

        if (!CheckIfProcessingIsOn())
            return;

        DamageTimer?.Start();

        ProcessEntityAndApplyDamage(body);
    }
    
    private void OnDamageTimerTimeout()
    {
        if (_currentTarget == null) return;
        
        ProcessEntityAndApplyDamage(_currentTarget as Node2D);
    }
    
    private void ProcessEntityAndApplyDamage(Node2D body)
    {
        if (body == null) return;

        if (!body.HasNode("HealthComponent")) return;
        var health = body.GetNode<HealthComponent>("HealthComponent");
        var inv = body.GetNodeOrNull<InvulnerabilityComponent>("InvulnerabilityComponent");

        if (inv != null && inv.IsInvulnerable())
            return;

        if (StatusEffectData != null && StatusEffectData.Type != StatusEffectType.None)
            EmitSignalEffectInflicted(body, StatusEffectData);

        DealDamage(health);

        inv?.Activate();
    }
    
    private bool CheckIfProcessingIsOn()
    {
        return ProcessMode is ProcessModeEnum.Inherit or ProcessModeEnum.Always;
    }
}