using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class KillPlayerOutOfScreenComponent : Node
{
    [Export] public VisibleOnScreenNotifier2D ScreenNotifier { get; set; }
    [Export] public HealthComponent HealthComponent { get; set; }

    private const float Damage = 6000f;
    
    public override void _Ready()
    {
        ScreenNotifier.ScreenExited += HandleOutOfScreen;
    }

    private void HandleOutOfScreen()
    {
        HealthComponent?.DecreaseHealth(Damage);
    }
}