using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.UI;

public partial class SkillButton : Button
{
    [Export] public SkillData Data { get; set; }

    public void Setup()
    {
        Icon = Data?.Icon;
        Text = Tr(Data?.Name) ?? string.Empty;
    }

    public void Activate()
    {
        Set("theme_override_colors/font_color", new Color("#49aa10"));
    }

    public void Deactivate()
    {
        Set("theme_override_colors/font_color", new Color("#ffffff"));
    }
}