using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

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
        _gameManager = GameManager.Instance;

        // Subscribe to lives changed event for reactive updates
        EventBus.Instance.LivesChanged += OnLivesChanged;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.LivesChanged -= OnLivesChanged;
        }
    }

    private void OnLivesChanged(int lives)
    {
        // Update the label when lives change
        LivesLeftLabel.Text = $" x {lives}";
    }

    private void SetLabels()
    {
        if (CurrentLevel != null)
        {
            CurrentLevelLabel.Text = CurrentLevel.LevelName;
        }

        // Read current lives from store
        var lives = GameStateStore.Instance?.Player.Lives ?? 0;
        LivesLeftLabel.Text = $" x {lives}";
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
        var lives = GameStateStore.Instance?.Player.Lives ?? 0;
        if (lives == 0) return;

        GetTree().ReloadCurrentScene();
    }
}