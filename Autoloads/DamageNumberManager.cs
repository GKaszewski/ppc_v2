using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.components;
using Mr.BrickAdventures.scripts.UI;

namespace Mr.BrickAdventures.Autoloads;

public partial class DamageNumberManager : Node
{
    [Export] public PackedScene DamageNumberScene { get; set; }
    
    private readonly List<Node> _managedNodes = [];
    
    public void Register(Node node)
    {
        if (_managedNodes.Contains(node)) return;
        
        var healthComponent = node.GetNodeOrNull<HealthComponent>("HealthComponent");
        if (healthComponent == null)
        {
            GD.PrintErr($"Node '{node.Name}' tried to register with DamageNumberManager but has no HealthComponent.");
            return;
        }

        healthComponent.HealthChanged += (delta, total) => OnHealthChanged(healthComponent, delta);
        node.TreeExiting += () => Unregister(node);
        _managedNodes.Add(node);
    }
    
    public void Unregister(Node node)
    {
        var healthComponent = node.GetNodeOrNull<HealthComponent>("HealthComponent");
        if (healthComponent != null)
        {
            healthComponent.HealthChanged -= (delta, _) => OnHealthChanged(healthComponent, delta);
        }
        _managedNodes.Remove(node);
    }

    private void OnHealthChanged(HealthComponent healthComponent, float delta)
    {
        if (delta >= 0) return;
        
        if (DamageNumberScene == null)
        {
            GD.PrintErr("DamageNumberManager: DamageNumberScene is not set!");
            return;
        }
        
        var damageNumber = DamageNumberScene.Instantiate<DamageNumber>();
        GetTree().CurrentScene.AddChild(damageNumber);
        
        var position = healthComponent.GlobalPosition;
        damageNumber.ShowDamage(Mathf.Abs(delta), position);
    }
}