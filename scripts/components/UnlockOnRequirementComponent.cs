using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

public partial class UnlockOnRequirementComponent : Node
{
    [Export] public RequirementComponent RequirementComponent { get; set; }
    [Export] public Node UnlockTarget { get; set; }

    public override void _Ready()
    {
        RequirementComponent.RequirementMet += OnRequirementMet;
    }

    private void OnRequirementMet(CollectableType requirementType)
    {
        if (requirementType != RequirementComponent.RequirementType) return;
        if (UnlockTarget is IUnlockable unlockable)
        {
            unlockable.Unlock();
        }
    }
}