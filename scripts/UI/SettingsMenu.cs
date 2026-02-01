using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class SettingsMenu : Control
{
    [Export] public Control InputSettingsControl { get; set; }
    [Export] public Control AudioSettingsControl { get; set; }
    [Export] public Control DisplaySettingsControl { get; set; }
    [Export] public Control GameplaySettingsControl { get; set; }
    [Export] public Control SettingsMenuControl { get; set; }
    [Export] public Button InputSettingsButton { get; set; }
    [Export] public Button AudioSettingsButton { get; set; }
    [Export] public Button DisplaySettingsButton { get; set; }
    [Export] public Button GameplaySettingsButton { get; set; }

    private UIManager _uiManager;

    public override void _Ready()
    {
        _uiManager = GetNode<UIManager>(Constants.UIManagerPath);

        InputSettingsButton.Pressed += OnInputSettingsPressed;
        AudioSettingsButton.Pressed += OnAudioSettingsPressed;
        DisplaySettingsButton.Pressed += OnDisplaySettingsPressed;
        GameplaySettingsButton.Pressed += OnGameplaySettingsPressed;

        InputSettingsControl?.Hide();
        AudioSettingsControl?.Hide();
        DisplaySettingsControl?.Hide();
        GameplaySettingsControl?.Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel")) return;
        if (_uiManager.IsScreenOnTop(SettingsMenuControl)) _uiManager.PopScreen();
    }

    private void OnInputSettingsPressed()
    {
        _uiManager.PushScreen(InputSettingsControl);
    }

    private void OnAudioSettingsPressed()
    {
        _uiManager.PushScreen(AudioSettingsControl);
    }

    private void OnDisplaySettingsPressed()
    {
        _uiManager.PushScreen(DisplaySettingsControl);
    }

    private void OnGameplaySettingsPressed()
    {
        _uiManager.PushScreen(GameplaySettingsControl);
    }
}