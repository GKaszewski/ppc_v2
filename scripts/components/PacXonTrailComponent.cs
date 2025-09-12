using System.Collections.Generic;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PacXonTrailComponent : Line2D
{
    private PacXonGridInteractor _gridInteractor;
    private readonly List<Vector2> _trailPoints = [];
    
    public void Initialize(PacXonGridInteractor interactor)
    {
        _gridInteractor = interactor;
        _gridInteractor.TrailStarted += OnTrailStarted;
        _gridInteractor.TrailExtended += OnTrailExtended;
        _gridInteractor.TrailCleared += OnTrailCleared;

        Width = 8;
        DefaultColor = new Color("#a6f684");
        JointMode = LineJointMode.Round;
        BeginCapMode = LineCapMode.Round;
        EndCapMode = LineCapMode.Round;
    }
    
    public override void _ExitTree()
    {
        if (_gridInteractor != null)
        {
            _gridInteractor.TrailStarted -= OnTrailStarted;
            _gridInteractor.TrailExtended -= OnTrailExtended;
            _gridInteractor.TrailCleared -= OnTrailCleared;
        }
    }
    
    private void OnTrailStarted(Vector2 startPosition)
    {
        _trailPoints.Clear();
        _trailPoints.Add(ToLocal(startPosition));
        _trailPoints.Add(ToLocal(startPosition));
        UpdateTrail();
    }

    private void OnTrailExtended(Vector2 newPosition)
    {
        if (_trailPoints.Count > 0)
        {
            _trailPoints[^1] = ToLocal(newPosition);
        }
        UpdateTrail();
    }

    private void OnTrailCleared()
    {
        _trailPoints.Clear();
        UpdateTrail();
    }

    private void UpdateTrail()
    {
        ClearPoints();
        foreach (var point in _trailPoints)
        {
            AddPoint(point);
        }
    }
}