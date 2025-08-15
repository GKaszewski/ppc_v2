using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.features.ui.menus;

[Meta(typeof(IAutoNode))]
public partial class MainMenu : Control
{
    public override void _Notification(int what) => this.Notify(what);
    
    [Node] private Button NewGameButton { get; set; } = null!;
    [Node] private Button ContinueButton { get; set; } = null!;
    [Node] private Button SettingsButton { get; set; } = null!;
    [Node] private Button CreditsButton { get; set; } = null!;
    [Node] private Button ExitButton { get; set; } = null!;
    [Node] private Label VersionLabel { get; set; } = null!;
    [Export] public Control SettingsControl { get; set; } = null!;
    [Export] public Control CreditsControl { get; set; } = null!;

    [Dependency] private SaveService Save => this.DependOn<SaveService>();
    [Dependency] private LevelRepository Levels => this.DependOn<LevelRepository>();
    [Dependency] private IGameScenes Scenes => this.DependOn<IGameScenes>();
    [Dependency] private ILevelCatalog Catalog => this.DependOn<ILevelCatalog>();

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
        var first = Catalog.First;
        Scenes.Load(first);
    }

    private void OnContinuePressed() {
        if (!Save.TryLoad()) { OnNewGamePressed(); return; }
        var scene = Catalog.Get(Levels.Current) ?? Catalog.First;
        Scenes.Load(scene);
    }
}