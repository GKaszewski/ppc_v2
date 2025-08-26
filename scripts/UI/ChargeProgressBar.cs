using Godot;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

public partial class ChargeProgressBar : ProgressBar
{
    [Export] public ProgressBar ProgressBar { get; set; }
    [Export] public BrickThrowComponent ThrowComponent { get; set; }
    
    private ChargeThrowInputResource _throwInput;

    public override void _Ready()
    {
        Owner.ChildEnteredTree += OnNodeEntered;   
        ProgressBar.Hide();
        SetupDependencies();
    }

    private void OnNodeEntered(Node node)
    {
        if (node is not BrickThrowComponent throwComponent || ThrowComponent != null) return;
        ThrowComponent = throwComponent;
        SetupDependencies();
    }

    private void SetupDependencies()
    {
        if (ThrowComponent == null || ProgressBar == null)
        {
            return;
        }
        
        if (ThrowComponent.ThrowInputBehavior is ChargeThrowInputResource throwInput)
        {
            _throwInput = throwInput;
        }
        else
        {
            _throwInput = null;
        }
        
        if (_throwInput == null)
        {
            return;
        }

        if (!_throwInput.SupportsCharging())
        {
            ProgressBar.Hide();
            return;
        }
        
        SetupProgressBar();
        
        _throwInput.ChargeStarted += OnChargeStarted;
        _throwInput.ChargeStopped += OnChargeStopped;
        _throwInput.ChargeUpdated += OnChargeUpdated;
    }

    private void SetupProgressBar()
    {
        ProgressBar.MinValue = _throwInput.MinPower;
        ProgressBar.MaxValue = _throwInput.MaxPower;
        ProgressBar.Value = _throwInput.MinPower;
        ProgressBar.Step = 0.01f;
        ProgressBar.Hide();
    }

    private void OnChargeStarted()
    {
        ProgressBar.Show();
    }

    private void OnChargeStopped()
    {
        ProgressBar.Hide();
    }

    private void OnChargeUpdated(float chargeRatio)
    {
        ProgressBar.Value = chargeRatio;
        ProgressBar.Show();
    }
}