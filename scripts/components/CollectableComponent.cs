using System;
using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class CollectableComponent : Node
{
    private bool _hasFadeAway = false;
    
    [Export] public Area2D Area2D { get; set; }
    [Export] public CollisionShape2D CollisionShape { get; set; }
    [Export] public CollectableResource Data { get; set; }
    [Export] public AudioStreamPlayer2D Sfx {get; set; }
    
    [Signal] public delegate void CollectedEventHandler(Variant amount, CollectableType type, Node2D body);

    public override void _Ready()
    {
        if (Area2D != null)
            Area2D.BodyEntered += OnArea2DBodyEntered;
        else
            GD.PushError("Collectable node missing Area2D node.");
        
        if (Owner.HasNode("FadeAwayComponent"))
            _hasFadeAway = true;
    }

    private async void OnArea2DBodyEntered(Node2D body)
    {
        try
        {
            if (!body.HasNode("CanPickUpComponent")) return;
            
            EmitSignalCollected(Data.Amount, Data.Type, body);
            CollisionShape?.CallDeferred("set_disabled", true);
            Sfx?.Play();

            if (_hasFadeAway) return;
                
            if (Sfx != null)
                await ToSignal(Sfx, AudioStreamPlayer2D.SignalName.Finished);
            Owner.QueueFree();
        }
        catch (Exception e)
        {
            GD.PushError($"Error in CollectableComponent.OnArea2DBodyEntered: {e.Message}");
        }
    }
}