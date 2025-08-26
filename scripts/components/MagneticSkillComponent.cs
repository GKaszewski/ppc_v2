using System;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class MagneticSkillComponent : Node, ISkill
{
    [Export] public Area2D MagneticArea { get; set; }
    [Export] public float MagneticMoveDuration { get; set; } = 1.25f;

    private Array<Node2D> _collectablesToPickUp = [];
    private Node2D _owner;

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
        if (!HasComponentInChildren(body, "Collectable")) return;

        if (_collectablesToPickUp.Contains(body)) return;
        _collectablesToPickUp.Add(body);
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (!HasComponentInChildren(area, "Collectable")) return;
        
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

    public void Initialize(Node owner)
    {
        _owner = owner as Node2D;
        if (_owner == null)
        {
            GD.PushWarning("MagneticSkillComponent: Owner is not a Node2D.");
        }
    }

    public void Activate()
    {
        MagneticArea.BodyEntered += OnBodyEntered;
        MagneticArea.AreaEntered += OnAreaEntered;
    }

    public void Deactivate()
    {
        MagneticArea.BodyEntered -= OnBodyEntered;
        MagneticArea.AreaEntered -= OnAreaEntered;
    }
}