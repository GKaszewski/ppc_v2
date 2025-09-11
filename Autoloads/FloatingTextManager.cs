using System.Globalization;
using Godot;
using Mr.BrickAdventures.scripts.UI;

namespace Mr.BrickAdventures.Autoloads;

public partial class FloatingTextManager : Node
{
    [Export] public PackedScene FloatingTextScene { get; set; }
    
    [ExportGroup("Colors")]
    [Export] public Color DamageColor { get; set; } = new Color("#b21030"); // Red
    [Export] public Color HealColor { get; set; } = new Color("#71f341");   // Green
    [Export] public Color CoinColor { get; set; } = new Color("#ebd320");   // Gold
    [Export] public Color MessageColor { get; set; } = new Color("#ffffff"); // White
    
    public void ShowDamage(float amount, Vector2 position)
    {
        var text = Mathf.Round(amount * 100f).ToString(CultureInfo.InvariantCulture);
        CreateFloatingText(text, position, DamageColor);
    }
    
    public void ShowHeal(float amount, Vector2 position)
    {
        var text = $"+{Mathf.Round(amount)}";
        CreateFloatingText(text, position, HealColor);
    }

    public void ShowCoin(int amount, Vector2 position)
    {
        var text = $"+{amount}";
        CreateFloatingText(text, position, CoinColor);
    }
    
    public void ShowMessage(string message, Vector2 position)
    {
        CreateFloatingText(message, position, MessageColor);
    }
    
    private void CreateFloatingText(string text, Vector2 position, Color color)
    {
        if (FloatingTextScene == null)
        {
            GD.PushError("FloatingTextManager: FloatingTextScene is not set!");
            return;
        }

        var popup = FloatingTextScene.Instantiate<FloatingText>();
        GetTree().CurrentScene.AddChild(popup);
        popup.Show(text, position, color);
    }
}