using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class PlayerController : Node2D
{
    [Export]
    public string DefaultMovementType { get; set; } = "platform";

    [Export]
    public Godot.Collections.Dictionary<string, NodePath> MovementTypes { get; set; }

    [Export]
    public Sprite2D ShipSprite { get; set; }

    private IMovement _currentMovement = null;
    [Signal]
    public delegate void MovementSwitchedEventHandler(string movementType);

    public override void _Ready()
    {
        base._Ready();

        foreach (var movementType in MovementTypes.Keys)
        {
            var movementNode = GetNodeOrNull(movementType);
            if (movementNode is IMovement playerMovement)
            {
                playerMovement.Enabled = false;
            }
        }

        SwitchMovement(DefaultMovementType);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventKey inputEventKey && inputEventKey.IsActionPressed("switch_movement"))
        {
            var nextMovementType = GetNextMovementType();
            SwitchMovement(nextMovementType);
        }
    }

    private void SwitchMovement(string movementType)
    {
        if (_currentMovement != null)
        {
            _currentMovement.Enabled = false;
        }

        if (MovementTypes.TryGetValue(movementType, out var movement))
        {
            _currentMovement = GetNodeOrNull<IMovement>(movement);
            if (_currentMovement == null)
            {
                GD.PushError($"Movement type '{movementType}' not found in MovementTypes.");
                return;
            }
            _currentMovement.Enabled = true;
            EmitSignalMovementSwitched(movementType);
        }
        else
        {
            GD.PushError($"Movement type '{movementType}' not found in MovementTypes.");
        }

        if (_currentMovement == null)
        {
            GD.PushError("No current movement set after switching.");
        }
    }

    private string GetNextMovementType()
    {
        var keys = new List<string>(MovementTypes.Keys);
        var currentIndex = keys.IndexOf(_currentMovement?.MovementType);

        if (currentIndex == -1)
        {
            return DefaultMovementType;
        }

        currentIndex = (currentIndex + 1) % keys.Count;
        return keys[currentIndex];
    }

    public void OnSpaceshipEntered()
    {
        SwitchMovement("ship");
        ShipSprite.Visible = true;
    }

    public void OnSpaceshipExited()
    {
        SwitchMovement(DefaultMovementType);
        ShipSprite.Visible = false;
    }
}