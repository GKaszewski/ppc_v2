using System.Linq;
using Godot;
using Mr.BrickAdventures.scripts.components;

namespace Mr.BrickAdventures.scripts;

[GlobalClass]
public partial class PacXonLevel : Node
{
    [Export] public PlayerController Player { get; set; }
    [Export] public PacXonGridManager GridManager { get; set; }
    [Export] public Node GhostContainer { get; set; }
    [Export] public Label PercentageLabel { get; set; }
    
    private const float WinPercentage = 0.90f;

    public override void _Ready()
    {
        var ghosts = GhostContainer.GetChildren().OfType<Node2D>().ToList();
        
        var playerMapPos = GridManager.LocalToMap(Player.Position);
        Player.GlobalPosition = GridManager.MapToLocal(playerMapPos);
        
        foreach (var ghost in ghosts)
        {
            var movement = ghost.GetNode<GhostMovementComponent>("GhostMovementComponent");
            movement?.Initialize(GridManager, Player);
        }
        
        Player.SetGridMovement();
        
        var gridMovement = Player.GetNodeOrNull<GridMovementAbility>("Movements/GridMovementAbility");
        var gridInteractor = Player.GetNodeOrNull<PacXonGridInteractor>("PacXonGridInteractor");
        var trailComponent = Player.GetNodeOrNull<PacXonTrailComponent>("PacXonTrailComponent");

        if (gridMovement != null && gridInteractor != null)
        {
            gridInteractor.Initialize(GridManager, gridMovement, ghosts);
            trailComponent?.Initialize(gridInteractor);
        }
        else
        {
            GD.PushError("Could not find GridMovementAbility or PacXonGridInteractor on Player.");
        }
        
        GridManager.FillPercentageChanged += OnFillPercentageChanged;
        OnFillPercentageChanged(GridManager.GetFillPercentage());
    }
    
    private void OnFillPercentageChanged(float percentage)
    {
        PercentageLabel.Text = $"Fill: {percentage:P0}";
        if (percentage >= WinPercentage)
        {
            GD.Print("YOU WIN!");
            GetTree().Paused = true;
        }
    }
}