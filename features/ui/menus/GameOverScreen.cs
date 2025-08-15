using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;

namespace Mr.BrickAdventures.features.ui.menus;

[Meta(typeof(IAutoNode))]
public partial class GameOverScreen : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Node] private Button RestartButton { get; set; } = null!;
    [Node] private Button MainMenuButton { get; set; } = null!;
    
    [Dependency] private IGameScenes Scenes => this.DependOn<IGameScenes>();
    [Dependency] private ILevelCatalog Catalog => this.DependOn<ILevelCatalog>();
    
    public void OnReady() {
        Visible = false;
        RestartButton.Pressed += () => Scenes.Restart();
        MainMenuButton.Pressed += () => Scenes.ReturnToMain(Catalog.MainMenu);
    }

    public void ShowGameOver() => Show();
}