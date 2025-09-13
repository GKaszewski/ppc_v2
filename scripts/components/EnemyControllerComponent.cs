using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class EnemyControllerComponent : Node
{
    [Export] public SideToSideMovementComponent MovementComponent { get; set; }
    [Export] public PeriodicShootingComponent ShootingComponent { get; set; }

    public override void _Process(double delta)
    {
        if (MovementComponent == null || ShootingComponent == null) return;

        if (MovementComponent.Direction != Vector2.Zero)
        {
            ShootingComponent.ShootDirection = MovementComponent.Direction;
        }
    }
}