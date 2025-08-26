using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

public partial class MarketplaceButton : Button
{
    [Export] public SkillData Data { get; set; }
    [Export] public Texture2D UnlockedSkillIcon { get; set; }
    [Export] public Texture2D LockedSkillIcon { get; set; }
    [Export] public Container SkillLevelContainer { get; set; }
    
    private GameManager _gameManager;
    private SkillUnlockerComponent _skillUnlockerComponent;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        
        Setup();
        var player = _gameManager.Player;

        var skillUnlockerComponent = player?.GetNodeOrNull<SkillUnlockerComponent>("SkillUnlockerComponent");
        if (skillUnlockerComponent == null) return;
        
        skillUnlockerComponent.SkillUnlocked += OnSkillUnlock;
    }

    public override void _ExitTree()
    {
        _skillUnlockerComponent.SkillUnlocked -= OnSkillUnlock;
    }

    private void Setup()
    {
        if (Data == null) return;
        
        for (var i = 0; i < Data.MaxLevel; i++)
        {
            var icon = new TextureRect()
            {
                Texture = i < Data.Level ? UnlockedSkillIcon : LockedSkillIcon,
            };
            SkillLevelContainer.AddChild(icon);
        }
    }

    private void OnSkillUnlock(SkillData skill)
    {
        if (skill.Name != Data.Name) return;

        for (var i = 0; i < Data.MaxLevel; i++)
        {
            var icon = SkillLevelContainer.GetChildOrNull<TextureRect>(i);
            if (icon == null) continue;
            icon.Texture = i < Data.Level ? UnlockedSkillIcon : LockedSkillIcon;
            Disabled = i >= Data.Level;
        }
    }
}