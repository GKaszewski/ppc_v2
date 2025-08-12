using System;
using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.scripts.components;

public partial class MagneticSkillComponent : Node
{
    [Export] public Area2D MagneticArea { get; set; }
    [Export] public float MagneticMoveDuration { get; set; } = 1.25f;

    private Array<Node2D> _collectablesToPickUp = [];

    public override void _Ready()
    {
        MagneticArea.AreaEntered += OnAreaEntered;
        MagneticArea.BodyEntered += OnBodyEntered;
    }

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
        if (!IsInstanceValid(collectable)) return;

        if (Owner is not Node2D root) return;
        
        var direction = (root.GlobalPosition - collectable.GlobalPosition).Normalized();
        var speed = direction.Length() / MagneticMoveDuration;

        collectable.GlobalPosition += direction.Normalized() * speed;
    }
}