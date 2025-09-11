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

    public override void _Ready()
    {
        InitializeGrid();
    }
    
    private void InitializeGrid()
    {
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
        SetCell(mapCoords, (int)state, Vector2I.Zero);
    }
}