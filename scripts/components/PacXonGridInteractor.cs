using System.Collections.Generic;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PacXonGridInteractor : Node
{
    private enum PlayerGridState { OnSolid, DrawingTrail }
    
    private PacXonGridManager _gridManager;
    private HealthComponent _healthComponent;
    private GridMovementAbility _gridMovement;
    
    private PlayerGridState _currentState = PlayerGridState.OnSolid;
    private readonly List<Vector2I> _currentTrail = [];
    private List<Node2D> _ghosts = [];
    
    [Signal] public delegate void TrailStartedEventHandler(Vector2 startPosition);
    [Signal] public delegate void TrailExtendedEventHandler(Vector2 newPosition);
    [Signal] public delegate void TrailClearedEventHandler();

    public override void _Ready()
    {
        _healthComponent = Owner.GetNodeOrNull<HealthComponent>("HealthComponent");
    }

    public void Initialize(PacXonGridManager gridManager, GridMovementAbility gridMovement, List<Node2D> ghosts)
    {
        _gridManager = gridManager;
        _gridMovement = gridMovement;
        _ghosts = ghosts;
        _gridMovement.Moved += OnPlayerMoved;
    }
    
    private void OnPlayerMoved(Vector2 newPosition)
    {
        if (_gridManager == null) return;
        
        var mapCoords = _gridManager.LocalToMap(newPosition);
        var destinationState = _gridManager.GetCellState(mapCoords);
        
        if (_currentState == PlayerGridState.DrawingTrail) EmitSignalTrailExtended(newPosition);

        if (destinationState == CellState.Trail)
        {
            EmitSignalTrailCleared();
            _healthComponent?.DecreaseHealth(9999);
            return;
        }

        if (_currentState == PlayerGridState.OnSolid)
        {
            if (destinationState == CellState.Empty)
            {
                // Moved from solid ground to an empty space, start drawing.
                _currentState = PlayerGridState.DrawingTrail;
                _currentTrail.Clear();
                _currentTrail.Add(mapCoords);
                _gridManager.SetCellState(mapCoords, CellState.Trail);
                EmitSignalTrailStarted(newPosition);
            }
        }
        else if (_currentState == PlayerGridState.DrawingTrail)
        {
            if (destinationState == CellState.Empty)
            {
                // Continue drawing the trail
                _currentTrail.Add(mapCoords);
                _gridManager.SetCellState(mapCoords, CellState.Trail);
            }
            else if (destinationState == CellState.Solid)
            {
                _gridManager.PerformFloodFill(_ghosts);
                GD.Print("Fill logic triggered!");
                _currentState = PlayerGridState.OnSolid;
                SolidifyTrail();
                _currentTrail.Clear();
                EmitSignalTrailCleared();
            }
        }
    }

    private void SolidifyTrail()
    {
        foreach (var pos in _currentTrail)
        {
            _gridManager.SetCellState(pos, CellState.Solid);
        }
    }
}