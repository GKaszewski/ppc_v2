using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class CageComponent : Node
{
    [Export] public LeverComponent Lever { get; set; }
    [Export] public Vector2 MoveValue { get; set; } = new(0, -100f);
    [Export] public float TweenDuration { get; set; } = 0.5f;
    [Export] public bool ShouldFree { get; set; } = true;
    
    private const string LeverGroupName = "levers";

    public override async void _Ready()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        if (Lever == null)
        {
            var leverNodes = GetTree().GetNodesInGroup(LeverGroupName);
            foreach (var leverNode in leverNodes)
            {
                var lever = leverNode.GetNodeOrNull<LeverComponent>("LeverComponent");
                if (lever != null) lever.Activated += OnLeverActivated;
            }
        }
    }

    private void OnLeverActivated()
    {
        var tween = CreateTween();
        if (Owner is Node2D root)
        {
            var endPosition = root.Position + MoveValue;
            tween.TweenProperty(root, "position", endPosition, TweenDuration);
        }

        tween.TweenCallback(Callable.From(OnTweenCompleted));

    }

    private void OnTweenCompleted()
    {
        if (!ShouldFree) return;
        Owner.QueueFree();
    }
}