using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class HazardComponent : Node2D
{
    [Export] public KnockbackComponent KnockbackComponent { get; set; }
    [Export] public Area2D HazardArea { get; set; }
    
    public override void _Ready()
    {
        if (KnockbackComponent == null)
        {
            GD.PrintErr("HazardComponent requires a KnockbackComponent to function properly.");
            SetProcess(false);
            return;
        }
        
        HazardArea.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Node {body.Name} entered hazard area.");
        if (body is CharacterBody2D characterBody && Owner is Node2D source)
        {
            KnockbackComponent.ApplyKnockback(characterBody, source);
        }
    }
}