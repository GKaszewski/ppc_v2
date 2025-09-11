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

    public override void _Ready()
    {
        _healthComponent = Owner.GetNodeOrNull<HealthComponent>("HealthComponent");
    }

    public void Initialize(PacXonGridManager gridManager, GridMovementAbility gridMovement)
    {
        _gridManager = gridManager;
        _gridMovement = gridMovement;
        _gridMovement.Moved += OnPlayerMoved;
    }
    
    private void OnPlayerMoved(Vector2 newPosition)
    {
        if (_gridManager == null) return;
        
        var mapCoords = _gridManager.LocalToMap(newPosition);
        var destinationState = _gridManager.GetCellState(mapCoords);

        if (destinationState == CellState.Trail)
        {
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
                // Reached solid ground, time to fill! (Logic to come)
                GD.Print("Fill logic triggered!");
                _currentState = PlayerGridState.OnSolid;
                SolidifyTrail();
                _currentTrail.Clear();
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