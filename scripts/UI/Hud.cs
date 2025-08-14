using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.scripts.UI;

public partial class Hud : Node
{
    [Export] public HealthComponent Health { get; set; }
    [Export] public Label CoinsLabel { get; set; }
    [Export] public ProgressBar HealthBar { get; set; }
    [Export] public Label LivesLabel { get; set; }

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public override void _Process(double delta)
    {
        SetHealthBar();
        SetLivesLabel();
        SetCoinsLabel();
    }

    private void SetCoinsLabel()
    {
        CoinsLabel.Text = Tr("COINS_LABEL") + ": " + _gameManager.GetCoins();
    }

    private void SetLivesLabel()
    {
        LivesLabel.Text = Tr("LIVES_LABEL") + ": " + _gameManager.GetLives();
    }

    private void SetHealthBar()
    {
        HealthBar.Value = Health.Health;
        HealthBar.MaxValue = Health.MaxHealth;
    }
}