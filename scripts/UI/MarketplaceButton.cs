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
        var player = _gameManager.Player;
        if (player == null) return;
        
        _skillUnlockerComponent = player.GetNodeOrNull<SkillUnlockerComponent>("SkillUnlockerComponent");
        if (_skillUnlockerComponent != null)
        {
            _skillUnlockerComponent.SkillUnlocked += OnSkillStateChanged;
        }
        
        UpdateButtonState();
    }

    public override void _ExitTree()
    {
        if (_skillUnlockerComponent != null)
        {
            _skillUnlockerComponent.SkillUnlocked -= OnSkillStateChanged;
        }
    }
    
    private void OnSkillStateChanged(SkillData skill)
    {
        if (skill.Name == Data.Name)
        {
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        if (Data == null || Data.Upgrades.Count == 0)
        {
            Visible = false;
            return;
        }

        var isUnlocked = _gameManager.IsSkillUnlocked(Data);
        
        for (var i = 0; i < SkillLevelContainer.GetChildCount(); i++)
        {
            SkillLevelContainer.GetChild(i).QueueFree();
        }

        for (var i = 0; i < Data.MaxLevel; i++)
        {
            var icon = new TextureRect()
            {
                Texture = (isUnlocked && i < Data.Level) ? UnlockedSkillIcon : LockedSkillIcon,
                ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional
            };
            SkillLevelContainer.AddChild(icon);
        }

        if (!isUnlocked)
        {
            Text = $"{Tr(Data.Name)} ({Data.Upgrades[0].Cost})";
            Disabled = false;
        }
        else if (Data.Level < Data.MaxLevel)
        {
            Text = $"{Tr(Data.Name)} ({Data.Upgrades[Data.Level].Cost})";
            Disabled = false;
        }
        else
        {
            Text = $"{Tr(Data.Name)} (MAX)";
            Disabled = true;
        }
    }
}