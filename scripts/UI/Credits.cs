using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class Credits : Control
{
    private UIManager _uiManager;

    public override void _Ready()
    {
        _uiManager = GetNode<UIManager>(Constants.UIManagerPath);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel")) return;
        if (_uiManager != null && _uiManager.IsScreenOnTop(this))
        {
            _uiManager.PopScreen();
        }
    }
}