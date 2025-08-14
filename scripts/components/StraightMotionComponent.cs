using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class StraightMotionComponent : Node
{
    [Export] public LaunchComponent LaunchComponent { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        var root = Owner as Node2D;
        if (root == null || LaunchComponent == null)
        {
            return;
        }
        
        root.Position += LaunchComponent.GetInitialVelocity() * (float)delta;
    }
}