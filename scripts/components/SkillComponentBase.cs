using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

/// <summary>
/// Base class for all skill components to reduce boilerplate.
/// </summary>
public abstract partial class SkillComponentBase : Node, ISkill
{
    protected PlayerController Player { get; private set; }
    protected SkillData Data { get; private set; }

    public virtual void Initialize(Node owner, SkillData data)
    {
        Player = owner as PlayerController;
        Data = data;

        if (Player == null)
        {
            GD.PrintErr($"{GetType().Name} must be a child of a PlayerController.");
        }
    }

    public abstract void Activate();
    public abstract void Deactivate();
    public virtual void ApplyUpgrade(SkillUpgrade upgrade) { }
}
