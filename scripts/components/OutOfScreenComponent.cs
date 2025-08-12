using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class OutOfScreenComponent : Node
{
    [Export] public VisibleOnScreenNotifier2D VisibilityNotifier { get; set; }

    public override void _Ready()
    {
        VisibilityNotifier.ScreenExited += OnScreenExited;
    }

    private void OnScreenExited()
    {
        Owner?.QueueFree();
    }
}