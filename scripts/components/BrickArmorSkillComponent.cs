using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BrickArmorSkillComponent : SkillComponentBase
{
    private HealthComponent _healthComponent;
    private float _armorBonus = 0;

    public override void Initialize(Node owner, SkillData data)
    {
        base.Initialize(owner, data);
        if (Player != null)
        {
            _healthComponent = Player.GetNode<HealthComponent>("HealthComponent");
        }
    }

    public override void Activate()
    {
        if (_healthComponent == null || Data == null) return;
        ApplyUpgrade(Data.Upgrades[Data.Level - 1]);
    }

    public override void Deactivate()
    {
        if (_healthComponent == null) return;
        _healthComponent.MaxHealth -= _armorBonus;
        if (_healthComponent.Health > _healthComponent.MaxHealth)
        {
            _healthComponent.SetHealth(_healthComponent.MaxHealth);
        }
        _armorBonus = 0;
    }

    public override void ApplyUpgrade(SkillUpgrade upgrade)
    {
        if (_healthComponent == null || upgrade == null) return;

        _healthComponent.MaxHealth -= _armorBonus;

        _armorBonus = (float)upgrade.Properties["health_bonus"];
        _healthComponent.MaxHealth += _armorBonus;
        _healthComponent.IncreaseHealth(_armorBonus);
    }
}