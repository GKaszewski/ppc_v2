using Godot;
using Mr.BrickAdventures.Autoloads;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class ScoreComponent : Node
{
    private GameManager _gameManager;
    private const string CoinsGroupName = "Coins";
    
    public override async void _Ready()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        
        _gameManager = GetNode<GameManager>("/root/GameManager");
        if (_gameManager == null)
        {
            GD.PrintErr("GameManager not found in the scene tree.");
            return;
        }

        var coins = GetTree().GetNodesInGroup("Coins");
        foreach (var coin in coins)
        {
            var c = coin.GetNodeOrNull<CollectableComponent>("CollectableComponent");
            if (c != null)
            {
                c.Collected += OnCollected;
            }
        }
    }

    private void OnCollected(Variant amount, CollectableType type, Node2D body)
    {
        if (type != CollectableType.Coin) return;
        
        var coinAmount = amount.As<int>();
        var currentCoins = (int)_gameManager.CurrentSessionState["coins_collected"];
        _gameManager.CurrentSessionState["coins_collected"] = currentCoins + coinAmount;
    }
}