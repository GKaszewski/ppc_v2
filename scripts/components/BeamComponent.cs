using Godot;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class BeamComponent : Node2D
{
    private float _currentLength = 0.0f;
    private const float PixelSize = 16.0f; // Assuming 16 pixels per unit for scaling
    
    [Export]
    public float ExpansionSpeed { get; set; } = 100.0f;
    [Export]
    public float MaxLength { get; set; } = 512.0f;
    [Export]
    public Vector2 Direction { get; set; } = Vector2.Down;
    [Export]
    public Node2D Root { get; set; }
    [Export]
    public Sprite2D Sprite { get; set; }
    [Export]
    public CollisionShape2D CollisionShape { get; set; }

    public override void _Ready()
    {
        if (Root == null)
        {
            GD.PrintErr("Root node is not set for BeamComponent.");
        }
        if (Sprite == null)
        {
            GD.PrintErr("Sprite node is not set for BeamComponent.");
        }
        if (CollisionShape == null)
        {
            GD.PrintErr("CollisionShape node is not set for BeamComponent.");
        }

        var shape = CollisionShape?.Shape as RectangleShape2D;
        shape?.SetSize(new Vector2(_currentLength / 2.0f, _currentLength / 2.0f));
        Sprite?.SetScale(new Vector2(1f, 1f));
        CollisionShape?.SetPosition(Vector2.Zero);
    }

    public override void _Process(double delta)
    {
        var newLength = _currentLength + ExpansionSpeed * (float)delta;
        if (newLength > MaxLength) newLength = MaxLength;
        
        if (!CheckForObstacle(newLength)) ExpandBeam(newLength);
    }

    private void ExpandBeam(float newLength)
    {
        _currentLength = newLength;

        if (Direction == Vector2.Up)
        {
            var pos = Sprite.Position;
            var scale = Sprite.Scale;
            var shape = CollisionShape?.Shape as RectangleShape2D;
            Sprite.SetScale(new Vector2(scale.X, _currentLength / PixelSize));
            Sprite.SetPosition(new Vector2(pos.X, -_currentLength  / 2.0f));
            shape?.SetSize(new Vector2(PixelSize / 2f, _currentLength / 2.0f));
            CollisionShape?.SetPosition(new Vector2(CollisionShape.Position.X, -_currentLength / 2.0f));
        } else if (Direction == Vector2.Down)
        {
            var pos = Sprite.Position;
            var scale = Sprite.Scale;
            var shape = CollisionShape?.Shape as RectangleShape2D;
            Sprite.SetScale(new Vector2(scale.X, _currentLength / PixelSize));
            Sprite.SetPosition(new Vector2(pos.X, _currentLength / 2.0f));
            shape?.SetSize(new Vector2(PixelSize / 2f, _currentLength / 2.0f));
            CollisionShape?.SetPosition(new Vector2(CollisionShape.Position.X, _currentLength / 2.0f));
        } else if (Direction == Vector2.Left)
        {
            var pos = Sprite.Position;
            var scale = Sprite.Scale;
            var shape = CollisionShape?.Shape as RectangleShape2D;
            Sprite.SetScale(new Vector2(_currentLength / PixelSize, scale.Y));
            Sprite.SetPosition(new Vector2(-_currentLength / 2.0f, pos.Y));
            shape?.SetSize(new Vector2(_currentLength / 2.0f, PixelSize / 2f));
            CollisionShape?.SetPosition(new Vector2(-_currentLength / 2.0f, CollisionShape.Position.Y));
        } else if (Direction == Vector2.Right)
        {
            var pos = Sprite.Position;
            var scale = Sprite.Scale;
            var shape = CollisionShape?.Shape as RectangleShape2D;
            Sprite.SetScale(new Vector2(_currentLength / PixelSize, scale.Y));
            Sprite.SetPosition(new Vector2(_currentLength / 2.0f, pos.Y));
            shape?.SetSize(new Vector2(_currentLength / 2.0f, PixelSize / 2f));
            CollisionShape?.SetPosition(new Vector2(_currentLength / 2.0f, CollisionShape.Position.Y));
        }
    }

    private bool CheckForObstacle(float newLength)
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var queryStart = GlobalPosition;
        var queryEnd = queryStart + Direction.Normalized() * newLength;
        var query = PhysicsRayQueryParameters2D.Create(queryStart, queryEnd);

        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        
        var result = spaceState.IntersectRay(query);
        return result.Count > 0;
    }
}