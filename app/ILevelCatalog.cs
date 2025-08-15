using Godot;

namespace Mr.BrickAdventures.app;

public interface ILevelCatalog
{
    int Count { get; }
    PackedScene? Get(int index);
    PackedScene First { get; }
    PackedScene MainMenu { get; }
}