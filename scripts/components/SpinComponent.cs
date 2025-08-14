using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class SpinComponent : Node
{
    [Export] public float SpinSpeed { get; set; } = 8f;
    [Export] public Vector2 SpinDirection { get; set; } = Vector2.Right;

    public override void _Process(double delta)
    {
        Spin((float)delta);
    }

    private void Spin(float delta)
    {
        var rotationSpeed = SpinSpeed * SpinDirection.X * delta;
        if (Owner is Node2D root)
        {
            root.Rotation += rotationSpeed;
        }
    }
}