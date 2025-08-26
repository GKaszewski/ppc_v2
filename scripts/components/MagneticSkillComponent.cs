using System;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class MagneticSkillComponent : Node, ISkill
{
    [Export] public Area2D MagneticArea { get; set; }
    [Export] public float MagneticMoveDuration { get; set; } = 1.25f;

    private Array<Node2D> _collectablesToPickUp = [];
    private Node2D _owner;
    private SkillData _skillData;

    public override void _Process(double delta)
    {
        foreach (var collectable in _collectablesToPickUp)
        {
            if (!IsInstanceValid(collectable))
            {
                _collectablesToPickUp.Remove(collectable);
                continue;
            }
            
            MoveCollectableToOwner(collectable);
        }
    }
    
    private void OnBodyEntered(Node2D body)
    {
        if (!HasComponentInChildren(body, "CollectableComponent")) return;

        if (_collectablesToPickUp.Contains(body)) return;
        _collectablesToPickUp.Add(body);
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (!HasComponentInChildren(area, "CollectableComponent")) return;
        
        if (_collectablesToPickUp.Contains(area)) return;
        _collectablesToPickUp.Add(area);
    }

    private bool HasComponentInChildren(Node node, string componentName)
    {
        if (node == null) return false;
        
        if (node.HasNode(componentName)) return true;

        foreach (var child in node.GetChildren())
        {
            if (child is { } childNode && HasComponentInChildren(childNode, componentName))
            {
                return true;
            }
        }
        
        return false;
    }

    private void MoveCollectableToOwner(Node2D collectable)
    {
        if (!IsInstanceValid(collectable) || !IsInstanceValid(_owner)) return;
        
        var direction = (_owner.GlobalPosition - collectable.GlobalPosition).Normalized();
        var speed = direction.Length() / MagneticMoveDuration;

        collectable.GlobalPosition += direction.Normalized() * speed;
    }

    public void Initialize(Node owner, SkillData data)
    {
        _owner = owner as Node2D;
        _skillData = data;
        
        if (_owner == null)
        {
            GD.PushWarning("MagneticSkillComponent: Owner is not a Node2D.");
        }

        if (MagneticArea == null)
        {
            if (owner is Area2D area2D) MagneticArea = area2D;
            else
            {
                MagneticArea = owner.GetNodeOrNull<Area2D>("MagneticArea");
                if (MagneticArea == null)
                {
                    GD.PushError("MagneticSkillComponent: MagneticArea is not set.");
                    return;
                }
            }
        }
        
        if (_skillData.Level > 0 && _skillData.Upgrades.Count >= _skillData.Level)
        {
            ApplyUpgrade(_skillData.Upgrades[_skillData.Level - 1]);
        }
    }

    public void Activate()
    {
        if (MagneticArea == null)
        {
            GD.PushError("MagneticSkillComponent: MagneticArea is not set.");
            return;
        }
        
        MagneticArea.BodyEntered += OnBodyEntered;
        MagneticArea.AreaEntered += OnAreaEntered;
    }

    public void Deactivate()
    {
        if (MagneticArea == null) return;
        
        MagneticArea.BodyEntered -= OnBodyEntered;
        MagneticArea.AreaEntered -= OnAreaEntered;
    }

    public void ApplyUpgrade(SkillUpgrade upgrade)
    {
        foreach (var property in upgrade.Properties)
        {
            Set(property.Key, property.Value);
        }
    }
}