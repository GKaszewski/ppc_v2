using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class LeverComponent : Node
{
    [Export] public Area2D Area { get; set; }
    [Export] public Sprite2D Sprite { get; set; }
    [Export] public int StartAnimationIndex { get; set; } = 0;
    [Export] public float AnimationDuration { get; set; } = 0.5f;
    [Export] public AudioStreamPlayer2D Sfx { get; set; }

    [Signal]
    public delegate void ActivatedEventHandler();

    public override void _Ready()
    {
        if (Area == null)
        {
            GD.PushError("LeverComponent: Area is not set.");
            return;
        }
        
        if (Sprite == null)
        {
            GD.PushError("LeverComponent: Sprite is not set.");
            return;
        }
        
        Area.BodyEntered += OnBodyEntered;
        Area.AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        HandleTriggerLogic(area);
    }

    private void OnBodyEntered(Node2D body)
    {
        HandleTriggerLogic(body);
    }

    private async Task Activate()
    {
        EmitSignalActivated();
        Sfx?.Play();
        Sprite.Frame = StartAnimationIndex + 1;
        var timer = GetTree().CreateTimer(AnimationDuration);
        await timer.ToSignal(timer, Timer.SignalName.Timeout);
        Sprite.Frame = StartAnimationIndex;
    }
    
    private void HandleTriggerLogic(Node2D obj)
    {
        var triggerLever = obj.GetNodeOrNull<TriggerLeverComponent>("TriggerLeverComponent");
        if (triggerLever == null)
        {
            GD.PushWarning("LeverComponent: TriggerLeverComponent not found in body.");
            return;
        }

        _ = Activate();
    }
}