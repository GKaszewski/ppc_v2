using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

public partial class ChargeProgressBar : ProgressBar
{
    [Export] public ProgressBar ProgressBar { get; set; }

    private SkillManager _skillManager;
    private BrickThrowComponent _throwComponent;
    private ChargeThrowInputResource _throwInput;

    public override void _Ready()
    {
        ProgressBar.Hide();

        _skillManager = GetNodeOrNull<SkillManager>(Constants.SkillManagerPath);
        if (_skillManager == null)
        {
            GD.PrintErr("ChargeProgressBar: SkillManager autoload not found.");
            return;
        }

        _skillManager.ActiveThrowSkillChanged += OnActiveThrowSkillChanged;

        SetupDependencies();
    }

    public override void _ExitTree()
    {
        if (_skillManager != null && IsInstanceValid(_skillManager))
        {
            _skillManager.ActiveThrowSkillChanged -= OnActiveThrowSkillChanged;
        }
        OnOwnerExiting();
    }

    private void OnActiveThrowSkillChanged(BrickThrowComponent throwComponent)
    {
        OnOwnerExiting();

        if (throwComponent == null || !IsInstanceValid(throwComponent)) return;

        _throwComponent = throwComponent;
        _throwComponent.TreeExiting += OnOwnerExiting;
        SetupDependencies();
    }

    private void OnOwnerExiting()
    {
        if (_throwInput != null)
        {
            _throwInput.ChargeStarted -= OnChargeStarted;
            _throwInput.ChargeStopped -= OnChargeStopped;
            _throwInput.ChargeUpdated -= OnChargeUpdated;
            _throwInput = null;
        }
        _throwComponent = null;
    }


    private void SetupDependencies()
    {
        if (_throwComponent == null || ProgressBar == null)
        {
            return;
        }

        if (_throwComponent.ThrowInputBehavior is ChargeThrowInputResource throwInput)
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