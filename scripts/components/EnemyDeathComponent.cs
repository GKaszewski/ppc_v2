using System.Threading.Tasks;
using Godot;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class EnemyDeathComponent : Node
{
    [Export] public float TweenDuration { get; set; } = 0.5f;
    [Export] public CollisionShape2D CollisionShape { get; set; }
    [Export] public HealthComponent Health { get; set; }

    public override void _Ready()
    {
        if (CollisionShape == null)
        {
            GD.PushError("EnemyDeathComponent: CollisionShape is not set.");
            return;
        }

        if (Health == null)
        {
            GD.PushError("EnemyDeathComponent: Health is not set.");
            return;
        }

        Health.Death += OnDeath;
    }

    private void OnDeath()
    {
        _ = Die();
    }

    private async Task Die()
    {
        // Emit enemy defeated event for statistics and other systems
        if (Owner is Node2D ownerNode)
        {
            EventBus.EmitEnemyDefeated(Owner, ownerNode.GlobalPosition);
        }

        CollisionShape.SetDisabled(true);
        var tween = CreateTween();
        tween.TweenProperty(Owner, "scale", Vector2.Zero, TweenDuration);
        await ToSignal(tween, Tween.SignalName.Finished);
        Owner.QueueFree();
    }
}