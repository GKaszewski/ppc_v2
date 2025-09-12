using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

public enum CellState
{
    Empty = -1,
    Solid = 0,
    Trail = 1,
    Hunted = 2
}

[GlobalClass]
public partial class PacXonGridManager : TileMapLayer
{
    [Export] public Rect2I PlayArea { get; set; } = new Rect2I(1, 1, 38, 28);
    
    private int _solidCellCount = 0;
    private int _totalPlayableCells = 0;
    
    [Signal] public delegate void FillPercentageChangedEventHandler(float percentage);

    public override void _Ready()
    {
        _totalPlayableCells = PlayArea.Size.X * PlayArea.Size.Y;
        InitializeGrid();
    }
    
    private void InitializeGrid()
    {
        Clear();
        
        for (var x = PlayArea.Position.X - 1; x <= PlayArea.End.X + 1; x++)
        {
            for (var y = PlayArea.Position.Y - 1; y <= PlayArea.End.Y + 1; y++)
            {
                if (x < PlayArea.Position.X || x > PlayArea.End.X || y < PlayArea.Position.Y || y > PlayArea.End.Y)
                {
                    SetCell(new Vector2I(x, y), (int)CellState.Solid, Vector2I.Zero);
                }
            }
        }
    }

    public CellState GetCellState(Vector2I mapCoords)
    {
        var tileId = GetCellSourceId(mapCoords);
        return (CellState)tileId;
    }

    public void SetCellState(Vector2I mapCoords, CellState state)
    {
        if (GetCellSourceId(mapCoords) != (int)CellState.Solid && state == CellState.Solid) _solidCellCount++;
        SetCell(mapCoords, (int)state, Vector2I.Zero);
    }
    
    public float GetFillPercentage()
    {
        return _totalPlayableCells > 0 ? (float)_solidCellCount / _totalPlayableCells : 0;
    }

    public void PerformFloodFill(List<Node2D> ghosts)
    {
        var unsafeCells = new HashSet<Vector2I>();

        foreach (var ghost in ghosts.Where(IsInstanceValid))
        {
            var ghostPos = LocalToMap(ghost.Position);
            FloodFillScan(ghostPos, unsafeCells);
        } 
        
        var filledCount = 0;
        for (var x = PlayArea.Position.X; x <= PlayArea.End.X; x++)
        {
            for (var y = PlayArea.Position.Y; y <= PlayArea.End.Y; y++)
            {
                var currentPos = new Vector2I(x, y);
                if (GetCellState(currentPos) == CellState.Empty && !unsafeCells.Contains(currentPos))
                {
                    SetCellState(currentPos, CellState.Solid);
                    filledCount++;
                }
            }
        }
        
        if (filledCount > 0)
        {
            EmitSignal(SignalName.FillPercentageChanged, GetFillPercentage());
        }
    }
    
    private void FloodFillScan(Vector2I startPos, HashSet<Vector2I> visited)
    {
        if (!PlayArea.HasPoint(startPos) || visited.Contains(startPos)) return;

        var q = new Queue<Vector2I>();
        q.Enqueue(startPos);
        visited.Add(startPos);

        while (q.Count > 0)
        {
            var pos = q.Dequeue();
            
            var neighbors = new[]
            {
                pos + Vector2I.Up, pos + Vector2I.Down, pos + Vector2I.Left, pos + Vector2I.Right
            };

            foreach (var neighbor in neighbors)
            {
                if (PlayArea.HasPoint(neighbor) && !visited.Contains(neighbor) && GetCellState(neighbor) == CellState.Empty)
                {
                    visited.Add(neighbor);
                    q.Enqueue(neighbor);
                }
            }
        }
    }
}