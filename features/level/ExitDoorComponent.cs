using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.features.level;

[Meta(typeof(IAutoNode))]
public partial class ExitDoorComponent : Area2D
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] public CollisionShape2D UnlockIndicator { get; set; } = null!;
    [Export] public bool Unlocked { get; set; } = false;

    [Dependency] public LevelService Levels => this.DependOn<LevelService>();
    [Dependency] public ILevelCatalog Catalog => this.DependOn<ILevelCatalog>();
    [Dependency] public IGameScenes Scenes => this.DependOn<IGameScenes>();

    public void OnReady() {
        BodyEntered += OnBodyEntered;
        UpdateVisuals();
    }

    private void OnBodyEntered(Node body) {
        if (!Unlocked) return;
        if (body is not CharacterBody2D) return;

        var nextIdx = Levels.CompleteAndAdvance();
        var next = Catalog.Get(nextIdx);
        if (next != null) Scenes.Load(next); else Scenes.ReturnToMain(Catalog.MainMenu);
    }

    private void UpdateVisuals() {
        if (UnlockIndicator != null) UnlockIndicator.Disabled = !Unlocked;
    }

    public void SetUnlocked(bool value) { Unlocked = value; UpdateVisuals(); }
}