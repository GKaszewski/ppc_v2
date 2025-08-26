using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

[GlobalClass]
public partial class DeathScreen : Control
{
    [Export] public LevelResource CurrentLevel { get; set; }
    [Export] public Label CurrentLevelLabel { get; set; }
    [Export] public Label LivesLeftLabel { get; set; }
    [Export] public float TimeoutTime { get; set; } = 2.0f;
    [Export] public Godot.Collections.Array<Node> NodesToDisable { get; set; } = new();
    
    private GameManager _gameManager;
    private Timer _timer;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        SetLabels();
    }
    
    private void SetLabels()
    {
        if (_gameManager == null) return;
        
        if (CurrentLevel != null)
        {
            CurrentLevelLabel.Text = CurrentLevel.LevelName;
        }
        LivesLeftLabel.Text = $" x {_gameManager.GetLives()}";
    }
    
    private void SetupTimer()
    {
        _timer = new Timer();
        _timer.WaitTime = TimeoutTime;
        _timer.OneShot = true;
        _timer.Timeout += OnTimeout;
        AddChild(_timer);
        _timer.Start();
    }
    
    private void ToggleNodes()
    {
        foreach (var node in NodesToDisable)
        {
            node.ProcessMode = node.ProcessMode == ProcessModeEnum.Disabled 
                ? ProcessModeEnum.Inherit 
                : ProcessModeEnum.Disabled;
        }
    }
    
    public void OnPlayerDeath()
    {
        if (_gameManager == null) return;
        
        ToggleNodes();
        SetLabels();
        Show();
        SetupTimer();
    }
    
    private void OnTimeout()
    {
        if (_gameManager == null || _gameManager.GetLives() == 0) return;
        
        GetTree().ReloadCurrentScene();
    }
}