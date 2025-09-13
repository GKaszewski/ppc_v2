using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

[GlobalClass]
public partial class SpeedRunHud : Control
{
    [Export] private Label _timerLabel;
    
    private SpeedRunManager _speedRunManager;

    public override void _Ready()
    {
        _speedRunManager = GetNode<SpeedRunManager>("/root/SpeedRunManager");
        
        _speedRunManager.TimeUpdated += OnTimerUpdated;
        
        Visible = _speedRunManager.IsVisible;
    }

    private void OnTimerUpdated(double totalTime, double levelTime)
    {
        _timerLabel.Text = SpeedRunManager.FormatTime(totalTime);
    }
}