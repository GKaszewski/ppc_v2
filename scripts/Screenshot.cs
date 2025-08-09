using Godot;

namespace Mr.BrickAdventures.scripts;

public partial class Screenshot : Node
{
    public override void _Process(double delta)
    {
        if (!OS.IsDebugBuild() || !Input.IsActionJustPressed("screenshot")) return;
        var img = GetViewport().GetTexture().GetImage();
        var id = OS.GetUniqueId() + "_" + Time.GetDatetimeStringFromSystem();
        var path = "user://screenshots/screenshot_" + id + ".png";
        img.SavePng(path);
    }
}