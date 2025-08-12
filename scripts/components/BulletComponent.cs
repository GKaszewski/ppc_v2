using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class BulletComponent : Node
{
    [Export] public Area2D Area { get; set; }
    [Export] public TerrainHitFx TerrainHitFx { get; set; }
    [Export] public Sprite2D BulletSprite { get; set; }
    
    public override void _Ready()
    {
        Area.BodyEntered += OnBodyEntered; 
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is TileMapLayer)
        {
            if (BulletSprite != null)
            {
                BulletSprite.Visible = false;
            }
            
            PlayTerrainHitFx();
            return;
        }
        
        Owner.QueueFree();
     }

    private void OnAreaEntered(Area2D area)
    {
        Owner.QueueFree();
    }

    private void PlayTerrainHitFx()
    {
        if (TerrainHitFx == null) return;
        
        TerrainHitFx.TriggerFx();
        
        Owner.QueueFree();
    }
}