using Godot;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.scripts;

[GlobalClass]
public partial class PacXonLevel : Node
{
    [Export] public PlayerController Player { get; set; }
    [Export] public PacXonGridManager GridManager { get; set; }

    public override void _Ready()
    {
        Player.SetGridMovement();
        
        var gridMovement = Player.GetNodeOrNull<GridMovementAbility>("Movements/GridMovementAbility");
        var gridInteractor = Player.GetNodeOrNull<PacXonGridInteractor>("PacXonGridInteractor");

        if (gridMovement != null && gridInteractor != null)
        {
            gridInteractor.Initialize(GridManager, gridMovement);
        }
        else
        {
            GD.PushError("Could not find GridMovementAbility or PacXonGridInteractor on Player.");
        }
    }
}