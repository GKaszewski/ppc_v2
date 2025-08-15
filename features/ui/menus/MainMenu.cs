using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.features.ui.menus;

[Meta(typeof(IAutoNode))]
public partial class MainMenu : Control
{
    public override void _Notification(int what) => this.Notify(what);
    
    [Export] public Control MainMenuControl { get; set; } = null!;
    [Export] public Button NewGameButton { get; set; } = null!;
    [Export] public Button ContinueButton { get; set; } = null!;
    [Export] public Button SettingsButton { get; set; } = null!;
    [Export] public Button CreditsButton { get; set; } = null!;
    [Export] public Button ExitButton { get; set; } = null!;
    [Export] public Label VersionLabel { get; set; } = null!;
    [Export] public Control SettingsControl { get; set; } = null!;
    [Export] public Control CreditsControl { get; set; } = null!;
    [Export] public PackedScene FirstLevelScene { get; set; } = null!;

    [Dependency] public SaveService Save => this.DependOn<SaveService>();
    [Dependency] public LevelRepository Levels => this.DependOn<LevelRepository>();

    public void OnReady() {
        VersionLabel.Text = $"v. {ProjectSettings.GetSetting("application/config/version")}";

        NewGameButton.Pressed += OnNewGamePressed;
        ContinueButton.Pressed += OnContinuePressed;
        SettingsButton.Pressed += () => SettingsControl.Show();
        CreditsButton.Pressed += () => CreditsControl.Show();
        ExitButton.Pressed += () => GetTree().Quit();
    }

    public void OnResolved()
    {
        ContinueButton.Disabled = !Save.Exists();
        (ContinueButton.Disabled ? NewGameButton : ContinueButton).GrabFocus();
    }

    private void OnNewGamePressed() {
        Save.NewGame();
        Levels.SetCurrent(0);
        GetTree().ChangeSceneToPacked(FirstLevelScene);
    }

    private void OnContinuePressed() {
        if (!Save.TryLoad()) {
            // Fallback: start new game
            OnNewGamePressed();
            return;
        }
        GetTree().ChangeSceneToPacked(FirstLevelScene);
    }
}