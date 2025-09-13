using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class CleanupOnCollisionComponent : Node
{
    [Export(PropertyHint.Range, "0, 5, 0.1")] public float CleanupDelay { get; set; } = 0.5f;
    
    private RigidBody2D _body;
    private CleanupComponent _cleanupComponent;
    private bool _isCleaningUp = false;
    
    public override void _Ready()
    {
        _body = Owner as RigidBody2D;
        if (_body == null)
        {
            GD.PrintErr("CleanupOnCollisionComponent must be attached to a RigidBody2D.");
            SetProcess(false);
            return;
        }

        _cleanupComponent = Owner.GetNode<CleanupComponent>("CleanupComponent");
        if (_cleanupComponent == null)
        {
            GD.PrintErr("CleanupOnCollisionComponent requires a CleanupComponent on the same node.");
            SetProcess(false);
            return;
        }

        _body.BodyEntered += (_) => OnBodyEntered();
    }
    
    private async void OnBodyEntered()
    {
        if (_isCleaningUp) return;
        
        _isCleaningUp = true;
        
        await ToSignal(GetTree().CreateTimer(CleanupDelay), Timer.SignalName.Timeout);

        _cleanupComponent?.CleanUp();
    }

}