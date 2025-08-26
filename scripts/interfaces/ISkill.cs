using Godot;

namespace Mr.BrickAdventures.scripts.interfaces;

public interface ISkill
{
    void Initialize(Node owner);
    
    void Activate();
    void Deactivate();
}