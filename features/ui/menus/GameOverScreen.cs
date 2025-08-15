using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;

namespace Mr.BrickAdventures.features.ui.menus;

[Meta(typeof(IAutoNode))]
public partial class GameOverScreen : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] public Button RestartButton { get; set; } = null!;
    [Export] public Button MainMenuButton { get; set; } = null!;
    [Export] public PackedScene MainMenuScene { get; set; } = null!;

    [Dependency] public IGameScenes Scenes => this.DependOn<IGameScenes>();

    public void OnReady() {
        Visible = false;
        RestartButton.Pressed += () => Scenes.Restart();
        MainMenuButton.Pressed += () => Scenes.ReturnToMain(MainMenuScene);
    }

    public void ShowGameOver() => Show();
}