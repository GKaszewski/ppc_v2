using Godot;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class RequirementComponent : Node
{
    [Export] public CollectableType RequirementType { get; set; }
    [Export] public int RequirementAmount { get; set; } = 1;

    private int _currentAmount = 0;
    
    private const string CollectableGroupName = "Collectables";

    [Signal]
    public delegate void RequirementMetEventHandler(CollectableType requirementType);

    public override void _Ready()
    {
        var collectables = GetTree().GetNodesInGroup(CollectableGroupName);
        foreach (var collectable in collectables)
        {
            var c = collectable.GetNodeOrNull<CollectableComponent>("CollectableComponent");
            if (c != null && c.Data.Type == RequirementType)
            {
                c.Collected += OnCollected;
            }
        }
    }

    private void OnCollected(Variant amount, CollectableType type, Node2D body)
    {
        AddProgress(amount.As<int>());
    }

    private void AddProgress(int amount = 1)
    {
        _currentAmount += amount;
        if (_currentAmount >= RequirementAmount)
        {
            EmitSignalRequirementMet(RequirementType);
        }
    }
}