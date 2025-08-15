using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

namespace Mr.BrickAdventures.features.player.skills;

[Meta(typeof(IAutoNode))]
public partial class MagneticEffect : Node, ISkillEffect
{
    public override void _Notification(int what) => this.Notify(what);

    [Node("%Player")] private Node2D Player { get; set; } = null!;

    [Export] public float BaseRadius  { get; set; } = 96f;
    [Export] public float BaseForce   { get; set; } = 600f;

    private int _level = 1;
    private float Radius => BaseRadius * (1.0f + 0.25f * (_level - 1));
    private float Force  => BaseForce  * (1.0f + 0.20f * (_level - 1));

    public void Configure(string id, int level) => _level = Mathf.Max(1, level);

    public override void _PhysicsProcess(double delta) {
        if (Player == null) return;

        var space = new World2D().DirectSpaceState;
        var shape = new CircleShape2D { Radius = Radius };
        var parms = new PhysicsShapeQueryParameters2D()
        {
            Shape = shape, Transform = Player.GlobalTransform
        };
        parms.CollideWithAreas = true;
        parms.CollideWithBodies = true;

        var hits = space.IntersectShape(parms, 32);
        foreach (var h in hits) {
            if (!h.TryGetValue("collider", out var obj) || obj.Obj is not Node2D n2d) continue;

            if (!n2d.HasNode("CollectableComponent")) continue;

            var dir = (Player.GlobalPosition - n2d.GlobalPosition).Normalized();
            n2d.GlobalPosition += dir * Force * (float)delta;
        }
    }
}