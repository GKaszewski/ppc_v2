using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class FlipComponent : Node2D
{
    [Export] public Sprite2D LeftEye { get; set; }
    [Export] public Sprite2D RightEye { get; set; }
    [Export] public PlayerController PlayerController { get; set; }

    public override void _Process(double delta)
    {
        if (PlayerController == null) return;

        var velocity = PlayerController.LastDirection;
        switch (velocity.X)
        {
            case < 0f:
                LeftEye.Frame = 1;
                RightEye.Frame = 1;
                LeftEye.FlipH = true;
                RightEye.FlipH = true;
                break;
            case > 0f:
                LeftEye.Frame = 1;
                RightEye.Frame = 1;
                LeftEye.FlipH = false;
                RightEye.FlipH = false;
                break;
            default:
                LeftEye.Frame = 0;
                RightEye.Frame = 0;
                break;
        }
    }
}