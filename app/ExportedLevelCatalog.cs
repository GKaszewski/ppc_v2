using Godot;
using Godot.Collections;
using Mr.BrickAdventures.app;

namespace Mr.BrickAdventures.common;

public sealed class ExportedLevelCatalog : ILevelCatalog
{
    private readonly Array<PackedScene> _levels;
    public PackedScene MainMenu { get; }
    public ExportedLevelCatalog(Array<PackedScene> levels, PackedScene mainMenu) {
        _levels = levels; MainMenu = mainMenu;
    }
    public int Count => _levels.Count;
    public PackedScene Get(int index) => (index >= 0 && index < _levels.Count) ? _levels[index] : null;
    public PackedScene First => _levels.Count > 0 ? _levels[0] : MainMenu;
}