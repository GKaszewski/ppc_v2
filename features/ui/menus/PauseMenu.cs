using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.features.ui.menus;

[Meta(typeof(IAutoNode))]
public partial class PauseMenu : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] public Button ResumeButton { get; set; } = null!;
    [Export] public Button RestartButton { get; set; } = null!;
    [Export] public Button MainMenuButton { get; set; } = null!;

    [Dependency] public IGameScenes Scenes => this.DependOn<IGameScenes>();
    [Dependency] public ILevelCatalog Catalog => this.DependOn<ILevelCatalog>();
    [Dependency] public LevelService Levels => this.DependOn<LevelService>();
    
    public void OnReady() {
        Visible = false;
        ResumeButton.Pressed += () => { GetTree().Paused = false; Hide(); };
        RestartButton.Pressed += () => { GetTree().Paused = false; Scenes.Restart(); };
        MainMenuButton.Pressed += () => { GetTree().Paused = false; Scenes.ReturnToMain(Catalog.MainMenu); };
    }

    public void Toggle() {
        if (Visible) { GetTree().Paused = false; Hide(); }
        else { Show(); GetTree().Paused = true; }
    }
}