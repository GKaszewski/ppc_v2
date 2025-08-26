using Godot;

namespace Mr.BrickAdventures.scripts.components;

public partial class TooltipComponent : Area2D
{
    [Export] public Control UiRoot { get; set; }
    [Export] public string Text { get; set; } = string.Empty;
    [Export] public Label TooltipLabel { get; set; }

    public override void _Ready()
    {
        TooltipLabel.Text = Text;
        UiRoot.Visible = false;
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        ShowTooltip();
    }
    
    private void OnBodyExited(Node2D body)
    {
        HideTooltip();
    }
    
    private void ShowTooltip()
    {
        UiRoot.Visible = true;
    }
    
    private void HideTooltip()
    {
        UiRoot.Visible = false;
    }
}