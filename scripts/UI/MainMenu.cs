using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class MainMenu : Control
{
    [Export] public Control MainMenuControl { get; set; }
    [Export] public Button NewGameButton { get; set; }
    [Export] public Button ContinueButton { get; set; }
    [Export] public Button SettingsButton { get; set; }
    [Export] public Button CreditsButton { get; set; }
    [Export] public Button ExitButton { get; set; }
    [Export] public Label VersionLabel { get; set; }
    [Export] public Control SettingsControl { get; set; }
    [Export] public Control CreditsControl { get; set; }
    
    private SaveSystem _saveSystem;
    private GameManager _gameManager;
    private UIManager _uiManager;

    public override void _Ready()
    {
        _saveSystem = GetNode<SaveSystem>("/root/SaveSystem");
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _uiManager = GetNode<UIManager>("/root/UIManager");

        NewGameButton.Pressed += OnNewGamePressed;
        ContinueButton.Pressed += OnContinuePressed;
        SettingsButton.Pressed += OnSettingsPressed;
        CreditsButton.Pressed += OnCreditsPressed;
        ExitButton.Pressed += OnExitPressed;

        VersionLabel.Text = $"v. {ProjectSettings.GetSetting("application/config/version")}";
        ContinueButton.Disabled = !_saveSystem.CheckSaveExists();

        if (_saveSystem.CheckSaveExists())
            ContinueButton.GrabFocus();
        else
            NewGameButton.GrabFocus();
    }

    private void OnExitPressed()
    {
        _gameManager.QuitGame();
    }

    private void OnCreditsPressed()
    {
        _uiManager.PushScreen(CreditsControl);
    }

    private void OnSettingsPressed()
    {
        _uiManager.PushScreen(SettingsControl);
    }

    private void OnContinuePressed()
    {
        _gameManager.ContinueGame();
    }

    private void OnNewGamePressed()
    {
        _gameManager.StartNewGame();
    }
}