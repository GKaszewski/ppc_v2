using System.Collections.Generic;
using Godot;
using Mr.BrickAdventures.scripts.interfaces;

namespace Mr.BrickAdventures.scripts.components;

public partial class PlayerController : CharacterBody2D
{
    [Export] private Node MovementAbilitiesContainer { get; set; }
    
    public Vector2 LastDirection { get; private set; } = Vector2.Right;
    public Vector2 PreviousVelocity { get; private set; } = Vector2.Zero;
    
    private List<MovementAbility> _abilities = [];
    private PlayerInputHandler _inputHandler;
    
    public override void _Ready()
    {
        _inputHandler = GetNode<PlayerInputHandler>("PlayerInputHandler");
        foreach (var child in MovementAbilitiesContainer.GetChildren())
        {
            if (child is MovementAbility ability)
            {
                _abilities.Add(ability);
            }
        }
    }
    
    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        
        foreach (var ability in _abilities)
        {
            velocity = ability.ProcessMovement(velocity, delta);
        }
        
        if (_inputHandler.MoveDirection.X != 0)
        {
            LastDirection = new Vector2(_inputHandler.MoveDirection.X > 0 ? 1 : -1, 0);
        }

        PreviousVelocity = Velocity;
        Velocity = velocity;
        MoveAndSlide();
    }
    
    public void AddAbility(MovementAbility ability)
    {
        _abilities.Add(ability);
        MovementAbilitiesContainer.AddChild(ability);
    }
    
    private void ClearMovementAbilities()
    {
        foreach (var ability in _abilities)
        {
            ability.QueueFree();
        }
        _abilities.Clear();
    }
    
    public void RemoveAbility<T>() where T : MovementAbility
    {
        for (var i = _abilities.Count - 1; i >= 0; i--)
        {
            if (_abilities[i] is T)
            {
                var ability = _abilities[i];
                _abilities.RemoveAt(i);
                ability.QueueFree();
                break;
            }
        }
    }

    public void SetPlatformMovement()
    {
        ClearMovementAbilities();
        
        AddAbility(new GroundMovementAbility());
        AddAbility(new JumpAbility());
        AddAbility(new GravityAbility());
    }
    
    public void SetSpaceshipMovement()
    {
        ClearMovementAbilities();
        
        AddAbility(new SpaceshipMovementAbility());
    }
}