using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BrickShieldSkillComponent : Node, ISkill
{
    [Export] public PackedScene ShieldScene { get; set; }
    
    private PlayerController _player;
    private Node2D _shieldInstance;
    private SkillData _skillData;
    private GameManager _gameManager;
    private SkillManager _skillManager;
    private HealthComponent _shieldHealth;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _skillManager = GetNode<SkillManager>("/root/SkillManager");
    }

    public void Initialize(Node owner, SkillData data)
    {
        _player = owner as PlayerController;
        _skillData = data;
    }

    public void Activate()
    {
        if (_player == null || ShieldScene == null || _shieldInstance != null) return;

        _shieldInstance = ShieldScene.Instantiate<Node2D>();
        _player.AddChild(_shieldInstance);
        _shieldInstance.Position = Vector2.Zero;
        _shieldInstance.TreeExiting += OnShieldDestroyed;
        _shieldHealth = _shieldInstance.GetNode<HealthComponent>("HealthComponent");
    }

    public void Deactivate()
    {
        if (_shieldInstance != null && IsInstanceValid(_shieldInstance))
        {
            _shieldInstance.TreeExiting -= OnShieldDestroyed;
            _shieldInstance.QueueFree();
        }
        _shieldInstance = null;
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
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
        if (_gameManager != null && _skillData != null && _skillManager != null)
        {
            _gameManager.RemoveSkill(_skillData.Name);
            _skillManager.RemoveSkill(_skillData.Name);
        }
        _shieldInstance = null;
    }
}