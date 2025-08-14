using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class ProjectileInitParams
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Direction { get; set; } = Vector2.Right;
    public float Rotation { get; set; } = 0f;
    public float PowerMultiplier { get; set; } = 1f;
}

public partial class ProjectileInitComponent : Node
{
    [Export] public LaunchComponent LaunchComponent { get; set; }

    public void Initialize(ProjectileInitParams p)
    {
        var position = p.Position;
        var direction = p.Direction;
        var rotation = p.Rotation;
        var power = p.PowerMultiplier;

        if (Owner is Node2D root)
        {
            root.GlobalPosition = position;
            root.GlobalRotation = rotation;
        }

        if (LaunchComponent == null) return;
        
        LaunchComponent.InitialDirection = direction;
        LaunchComponent.SpawnPosition = position;
        LaunchComponent.SpawnRotation = rotation;
        LaunchComponent.Speed *= power;
    }
}