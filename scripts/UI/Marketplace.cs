using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

public partial class Marketplace : Control
{
    [Export] public Array<SkillData> Skills { get; set; } = [];
    [Export] public GridContainer ToUnlockGrid { get; set; }
    [Export] public GridContainer UnlockedGrid { get; set; }
    [Export] public Font Font { get; set; }
    [Export] public SkillUnlockerComponent SkillUnlockerComponent { get; set; }
    [Export] public Array<Node> ComponentsToDisable { get; set; } = [];
    [Export] public PackedScene MarketplaceButtonScene { get; set; }
    [Export] public PackedScene SkillButtonScene { get; set; }
    
    private GameManager _gameManager;
    private readonly List<Button> _unlockButtons = [];
    private readonly List<SkillButton> _skillButtons = [];

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        
        var skillsToUnlock = new List<SkillData>();
        
        foreach (var skill in Skills) skillsToUnlock.Add(skill);

        foreach (var skill in skillsToUnlock) CreateUpgradeButton(skill);

        var unlockedSkills = _gameManager.GetUnlockedSkills();
        foreach (var skill in unlockedSkills) CreateSkillButton(skill);
        
        SkillUnlockerComponent.SkillUnlocked += OnSkillUnlocker;
    }

    public override void _ExitTree()
    {
        SkillUnlockerComponent.SkillUnlocked -= OnSkillUnlocker;
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("show_marketplace")) return;
        
        if (IsVisible())
        {
            Hide();
            foreach (var c in ComponentsToDisable) c.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            Show();
            foreach (var c in ComponentsToDisable) c.ProcessMode = ProcessModeEnum.Disabled;
        }
    }

    private string GetButtonText(SkillData skill)
    {
        return $"{Tr(skill.Name)} {skill.Cost}";
    }

    private void OnSkillUnlocker(SkillData skill)
    {
        if (_skillButtons.Count == 0) CreateSkillButton(skill);

        foreach (var btn in _skillButtons)
        {
            if (btn.Data.IsActive)
                btn.Activate();
            else
                btn.Deactivate();
        }
    }

    private void CreateSkillButton(SkillData skill)
    {
        var button = SkillButtonScene.Instantiate<SkillButton>();
        button.Data = skill;
        button.Setup();
        button.Pressed += () => OnSkillButtonPressed(button);
        button.Activate();
        
        _skillButtons.Add(button);
        UnlockedGrid.AddChild(button);
        UnlockedGrid.QueueSort();
    }

    private void CreateUpgradeButton(SkillData skill)
    {
        var button = MarketplaceButtonScene.Instantiate<MarketplaceButton>();
        button.Text = GetButtonText(skill);
        button.Data = skill;
        button.Icon = skill.Icon;
        button.Pressed += () => OnUpgradeButtonPressed(skill);
        
        _unlockButtons.Add(button);
        ToUnlockGrid.AddChild(button);
        ToUnlockGrid.QueueSort();
    }

    private void OnUpgradeButtonPressed(SkillData skill)
    {
        if (_gameManager.IsSkillUnlocked(skill))
        {
            if (skill.Level < skill.MaxLevel)
            {
                SkillUnlockerComponent.TryUpgradeSkill(skill);
                if (!skill.IsActive) SkillUnlockerComponent.SkillManager.ToggleSkillActivation(skill);
            }
            else
            {
                SkillUnlockerComponent.SkillManager.ToggleSkillActivation(skill);
            }
        }
        else
        {
            SkillUnlockerComponent.TryUnlockSkill(skill);
        }
    }

    private void RemoveButton(SkillData skill)
    {
        foreach (var node in ToUnlockGrid.GetChildren())
        {
            var child = (Button)node;
            if (child.Text != GetButtonText(skill)) continue;
            
            child.QueueFree();
            break;
        }
    }

    private void OnSkillButtonPressed(SkillButton button)
    {
        SkillUnlockerComponent.SkillManager.ToggleSkillActivation(button.Data);
    }
}