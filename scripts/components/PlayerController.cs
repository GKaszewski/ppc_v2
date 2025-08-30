using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class PlayerController : CharacterBody2D
{
    [Export] private Node MovementAbilitiesContainer { get; set; }
    
    [ExportGroup("Movement Ability Scenes")]
    [Export] public PackedScene GroundMovementScene { get; set; }
    [Export] public PackedScene JumpMovementScene { get; set; }
    [Export] public PackedScene GravityScene { get; set; }
    [Export] public PackedScene OneWayPlatformScene { get; set; }
    [Export] public PackedScene SpaceshipMovementScene { get; set; }
    [Export] public PackedScene WallJumpScene { get; set; }
    
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
                ability.Initialize(this);
                _abilities.Add(ability);
            }
        }
        
        _ = ConnectJumpAndGravityAbilities();
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
        MovementAbilitiesContainer.AddChild(ability);
        ability.Initialize(this);
        _abilities.Add(ability);
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
        
        if (GroundMovementScene != null) AddAbility(GroundMovementScene.Instantiate<MovementAbility>());
        if (JumpMovementScene != null) AddAbility(JumpMovementScene.Instantiate<MovementAbility>());
        if (GravityScene != null) AddAbility(GravityScene.Instantiate<MovementAbility>());
        if (OneWayPlatformScene != null) AddAbility(OneWayPlatformScene.Instantiate<MovementAbility>());
        
        _ = ConnectJumpAndGravityAbilities();
    }
    
    public void SetSpaceshipMovement()
    {
        ClearMovementAbilities();
        
        if (SpaceshipMovementScene != null) AddAbility(SpaceshipMovementScene.Instantiate<MovementAbility>());
    }
    
    private async Task ConnectJumpAndGravityAbilities()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        
        var jumpAbility = _abilities.OfType<VariableJumpAbility>().FirstOrDefault();
        var gravityAbility = _abilities.OfType<GravityAbility>().FirstOrDefault();
        
        if (jumpAbility != null && gravityAbility != null)
        {
            gravityAbility.AscendGravity = jumpAbility.AscendGravity;
            gravityAbility.DescendGravity = jumpAbility.DescendGravity;
        }
    }
}