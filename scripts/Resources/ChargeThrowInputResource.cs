using Godot;

namespace Mr.BrickAdventures.scripts.Resources;

public partial class ChargeThrowInputResource : ThrowInputResource
{
    [Export] public float MinPower { get; set; } = 0.5f;
    [Export] public float MaxPower { get; set; } = 2.0f;
    [Export] public float MaxChargeTime { get; set; } = 2.0f;
    [Export] public float MinChargeDuration { get; set; } = 0.1f;
    
    private bool _isCharging = false;
    private float _chargeStartTime = 0f;
    
    [Signal] public delegate void ChargeStartedEventHandler();
    [Signal] public delegate void ChargeUpdatedEventHandler(float chargeRatio);
    [Signal] public delegate void ChargeStoppedEventHandler();

    public override void ProcessInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            _isCharging = true;
            _chargeStartTime = Time.GetTicksMsec() / 1000f;
            EmitSignalChargeStarted();
        }

        if (@event.IsActionReleased("attack") && _isCharging)
        {
            var power = CalculatePower();
            _isCharging = false;
            EmitSignalThrowRequested(power);
            EmitSignalChargeStopped();
        }
    }

    public override void Update(double delta)
    {
        if (!_isCharging) return;
        
        var t = Mathf.Clamp(GetChargeRatio(), MinPower, MaxPower);
        EmitSignalChargeUpdated(t);
    }

    public override bool SupportsCharging()
    {
        return true;
    }

    private float CalculatePower()
    {
        var now = Time.GetTicksMsec() / 1000f;
        var heldTime = now - _chargeStartTime;
        if (heldTime < MinChargeDuration) 
            return MinPower;
        
        var t = Mathf.Clamp(heldTime / MaxChargeTime, 0f, 1f);
        return Mathf.Lerp(MinPower, MaxPower, t);
    }

    private float GetChargeRatio()
    {
        if (!_isCharging) return MinPower;
        
        var now = Time.GetTicksMsec() / 1000f;
        var heldTime = now - _chargeStartTime;
        var t = Mathf.Clamp(heldTime / MaxChargeTime, 0f, 1f);
        return Mathf.Lerp(MinPower, MaxPower, t);
    }
}