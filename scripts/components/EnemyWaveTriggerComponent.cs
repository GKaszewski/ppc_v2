using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class EnemyWaveTriggerComponent : Node
{
    [Export] public Area2D Area2D { get; set; }
    [Export] public PathFollow2D PathFollowNode { get; set; }
    [Export] public float Speed { get; set; } = 100f;
    [Export] public bool Loop { get; set; } = false;
    [Export] public bool ActivateOnEnter { get; set; } = true;
    
    private bool _isActive = false;

    public override void _Ready()
    {
        Area2D.BodyEntered += OnBodyEntered;
        
        if (PathFollowNode == null) return;
        
        PathFollowNode.SetProgress(0f);
        PathFollowNode.SetProcess(false);
    }

    public override void _Process(double delta)
    {
        if (!_isActive || PathFollowNode == null) return;
        
        var progress = PathFollowNode.Progress;
        progress += (float)(delta * Speed);
        PathFollowNode.SetProgress(progress);

        if (!(PathFollowNode.ProgressRatio >= 1f) || Loop) return;
        
        _isActive = false;
        PathFollowNode.SetProcess(false);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (ActivateOnEnter) StartWave();
    }

    private void StartWave()
    {
        if (PathFollowNode == null) return;
        
        PathFollowNode.SetProcess(true);
        _isActive = true;
    }
}