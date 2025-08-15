using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.features.ui.hud;

[Meta(typeof(IAutoNode))]
public partial class Hud : Node
{
    public override void _Notification(int what) => this.Notify(what);
    
    [Export] public HealthComponent Health { get; set; } = null!;
    [Node] public Label CoinsLabel { get; set; } = null!;
    [Node] public ProgressBar HealthBar { get; set; } = null!;
    [Node] public Label LivesLabel { get; set; } = null!;

    [Dependency] public PlayerRepository Player => this.DependOn<PlayerRepository>();

    public void OnResolved() {
        CoinsLabel.Text = $"{Tr("COINS_LABEL")}: {Player.Coins}";
        LivesLabel.Text = $"{Tr("LIVES_LABEL")}: {Player.Lives}";

        Player.CoinsChanged += c => CoinsLabel.Text = $"{Tr("COINS_LABEL")}: {c}";
        Player.LivesChanged += l => LivesLabel.Text = $"{Tr("LIVES_LABEL")}: {l}";
        
        if (Health != null) {
            HealthBar.MaxValue = Health.MaxHealth;
            HealthBar.Value = Health.Health;
            Health.HealthChanged += (_, total) => { HealthBar.Value = total; };
        }
    }
}