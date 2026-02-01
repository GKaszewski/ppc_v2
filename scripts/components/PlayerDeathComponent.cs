using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PlayerDeathComponent : Node2D
{
    [Export] public AudioStreamPlayer2D DeathSfx { get; set; }
    [Export] public PackedScene DeathEffect { get; set; }
    [Export] public HealthComponent HealthComponent { get; set; }
    [Export] public Vector2 EffectScale { get; set; } = new Vector2(1.5f, 1.5f);

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GameManager.Instance;
        HealthComponent.Death += OnDeath;
    }

    private void OnDeath()
    {
        DeathSfx?.Play();

        if (DeathEffect != null)
        {
            var effect = DeathEffect.Instantiate<Node2D>();
            GetParent().AddChild(effect);
            effect.GlobalPosition = GlobalPosition;
            effect.Scale = EffectScale;
        }

        // Lives are now decremented by LivesStateHandler via PlayerDied event
        // Session state is reset by LivesStateHandler as well
    }
}