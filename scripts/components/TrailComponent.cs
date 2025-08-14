using System.Collections.Generic;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class TrailComponent : Line2D
{
    [Export] public int MaxPoints { get; set; } = 100;

    private readonly Queue<Vector2> _queue = new();

    public override void _Process(double delta)
    {
        if (Owner is Node2D root) _queue.Enqueue(root.GlobalPosition);
        
        if (_queue.Count > MaxPoints) _queue.Dequeue();
        
        ClearPoints();

        foreach (var point in _queue) AddPoint(point);
    }
}