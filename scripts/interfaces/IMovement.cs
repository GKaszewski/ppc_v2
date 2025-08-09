using Godot;

namespace Mr.BrickAdventures.scripts.interfaces;

public interface IMovement
{
    string MovementType { get; }
    bool Enabled { get; set; }
    Vector2 PreviousVelocity { get; set; }
    
    void _Process(double delta);
    void _PhysicsProcess(double delta);
}