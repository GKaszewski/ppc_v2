using System.Linq;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PlayerGraphicsComponent : Node
{
    [Export] public Node2D DefaultSprite { get; set; }
    [Export] public Node2D SpaceshipSprite { get; set; }

    private PlayerController _playerController;
    
    public override void _Ready()
    {
        _playerController = GetOwner<PlayerController>();
        if (_playerController == null)
        {
            GD.PrintErr("PlayerGraphicsComponent must be a child of a PlayerController.");
            SetProcess(false);
            return;
        }

        _playerController.MovementAbilitiesChanged += OnMovementAbilitiesChanged;
        
        UpdateGraphics();
    }

    private void OnMovementAbilitiesChanged()
    {
        UpdateGraphics();
    }
    
    private void UpdateGraphics()
    {
        var isSpaceship = _playerController.GetActiveAbilities().Any(ability => ability is SpaceshipMovementAbility);

        if (SpaceshipSprite != null)
        {
            SpaceshipSprite.Visible = isSpaceship;
        }
    }
}