using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class PauseMenu : Control
{
    [Export] public Control PauseMenuControl { get; set; }
    [Export] public Control SettingsControl { get; set; }
    [Export] public Button ResumeButton { get; set; }
    [Export] public Button MainMenuButton { get; set; }
    [Export] public Button QuitButton { get; set; }
    [Export] public Button SettingsButton { get; set; }
    [Export] public PackedScene MainMenuScene { get; set; }

    private GameManager _gameManager;
    private UIManager _uiManager;

    public override void _Ready()
    {
        _gameManager = GameManager.Instance;
        _uiManager = GetNode<UIManager>(Constants.UIManagerPath);

        ResumeButton.Pressed += OnResumePressed;
        MainMenuButton.Pressed += OnMainMenuPressed;
        QuitButton.Pressed += OnQuitPressed;
        SettingsButton.Pressed += OnSettingsPressed;

        PauseMenuControl.Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("pause")) return;
        if (_uiManager.IsVisibleOnStack(PauseMenuControl))
            OnResumePressed();
        else
        {
            _uiManager.PushScreen(PauseMenuControl);
            _gameManager.PauseGame();
        }
    }

    private void OnSettingsPressed()
    {
        _uiManager.PushScreen(SettingsControl);
        _gameManager.PauseGame();
    }

    private void OnQuitPressed()
    {
        _gameManager.QuitGame();
    }

    private void OnMainMenuPressed()
    {
        _gameManager.ResumeGame();
        _gameManager.ResetCurrentSessionState();
        GetTree().ChangeSceneToPacked(MainMenuScene);
    }

    private void OnResumePressed()
    {
        _uiManager.PopScreen();
        _gameManager.ResumeGame();
    }
}