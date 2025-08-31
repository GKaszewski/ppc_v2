using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BrickArmorSkillComponent : Node, ISkill
{
    private HealthComponent _healthComponent;
    private SkillData _skillData;
    private float _armorBonus = 0;
    
    public void Initialize(Node owner, SkillData data)
    {
        if (owner is not PlayerController player) return;
        _healthComponent = player.GetNode<HealthComponent>("HealthComponent");
        _skillData = data;
    }

    public void Activate()
    {
        if (_healthComponent == null || _skillData == null) return;
        ApplyUpgrade(_skillData.Upgrades[_skillData.Level - 1]);
    }

    public void Deactivate()
    {
        if (_healthComponent == null) return;
        _healthComponent.MaxHealth -= _armorBonus;
        if (_healthComponent.Health > _healthComponent.MaxHealth)
        {
            _healthComponent.SetHealth(_healthComponent.MaxHealth);
        }
        _armorBonus = 0;
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
    {
        if (_healthComponent == null || upgrade == null) return;

        _healthComponent.MaxHealth -= _armorBonus;

        _armorBonus = (float)upgrade.Properties["health_bonus"];
        _healthComponent.MaxHealth += _armorBonus;
        _healthComponent.IncreaseHealth(_armorBonus);
    }
}