using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class UnlockOnRequirementComponent : Node
{
    [Export] public RequirementComponent RequirementComponent { get; set; }
    [Export] public Node UnlockTarget { get; set; }

    public override void _Ready()
    {
        RequirementComponent.RequirementMet += OnRequirementMet;

        if (RequirementComponent.RequirementType == CollectableType.None)
        {
            OnRequirementMet(RequirementComponent.RequirementType);
        }
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