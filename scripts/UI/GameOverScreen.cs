using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class GameOverScreen : Control
{
    [Export] public Control GameOverPanel { get; set; }
    [Export] public Button RestartButton { get; set; }
    [Export] public Button MainMenuButton { get; set; }
    [Export] public PackedScene MainMenuScene { get; set; }

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GameManager.Instance;
        RestartButton.Pressed += OnRestartClicked;
        MainMenuButton.Pressed += OnMainMenuClicked;
    }

    private void OnMainMenuClicked()
    {
        _gameManager.ResetPlayerState();
        GetTree().ChangeSceneToPacked(MainMenuScene);
    }

    private void OnRestartClicked()
    {
        _gameManager.RestartGame();
    }

    public void OnPlayerDeath()
    {
        if (_gameManager == null || _gameManager.GetLives() != 0) return;

        GameOverPanel.Show();
    }
}