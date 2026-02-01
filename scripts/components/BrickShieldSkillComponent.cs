using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BrickShieldSkillComponent : SkillComponentBase
{
    [Export] public PackedScene ShieldScene { get; set; }

    private Node2D _shieldInstance;
    private GameManager _gameManager;
    private SkillManager _skillManager;
    private HealthComponent _shieldHealth;

    public override void _Ready()
    {
        _gameManager = GameManager.Instance;
        _skillManager = SkillManager.Instance;
    }

    public override void Activate()
    {
        if (Player == null || ShieldScene == null || _shieldInstance != null) return;

        _shieldInstance = ShieldScene.Instantiate<Node2D>();
        Player.AddChild(_shieldInstance);
        _shieldInstance.Position = Vector2.Zero;
        _shieldInstance.TreeExiting += OnShieldDestroyed;
        _shieldHealth = _shieldInstance.GetNode<HealthComponent>("HealthComponent");
    }

    public override void Deactivate()
    {
        if (_shieldInstance != null && IsInstanceValid(_shieldInstance))
        {
            _shieldInstance.TreeExiting -= OnShieldDestroyed;
            _shieldInstance.QueueFree();
        }
        _shieldInstance = null;
    }

    public override void ApplyUpgrade(SkillUpgrade upgrade)
    {
        upgrade.Properties.TryGetValue("shield_health", out var newHealth);
        if (_shieldHealth != null)
        {
            _shieldHealth.Health = (float)newHealth;
            _shieldHealth.MaxHealth = (float)newHealth;
        }
    }

    private void OnShieldDestroyed()
    {
        if (_gameManager != null && Data != null && _skillManager != null)
        {
            _gameManager.RemoveSkill(Data.Name);
            _skillManager.RemoveSkill(Data.Name);
        }
        _shieldInstance = null;
    }
}