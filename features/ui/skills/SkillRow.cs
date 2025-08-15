using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.features.ui.skills;

[Meta(typeof(IAutoNode))]
public partial class SkillRow : Control
{
    public override void _Notification(int what) => this.Notify(what);

  [Node] private Label Name { get; set; } = null!;
  [Node] private Label Level { get; set; } = null!;
  [Node] private Label Cost  { get; set; } = null!;
  [Node] private Button Buy  { get; set; } = null!;
  [Node] private Button Toggle { get; set; } = null!;
  [Node] private TextureRect Icon { get; set; } = null!;

  [Dependency] private SkillsRepository Skills => this.DependOn<SkillsRepository>();
  [Dependency] private SkillService Service    => this.DependOn<SkillService>();
  [Dependency] private ISkillCatalog Catalog   => this.DependOn<ISkillCatalog>();

  public string SkillId { get; set; } = null!;

  public void Init(string id) {
    SkillId = id;
  }

  public void OnResolved() {
    if (!Catalog.TryGet(SkillId, out var def)) { QueueFree(); return; }

    Name.Text = Tr(def.DisplayName);
    UpdateUi();

    Buy.Pressed += OnBuyPressed;
    Toggle.Pressed += OnTogglePressed;

    // react to repo changes
    Skills.Unlocked      += OnUnlocked;
    Skills.LevelChanged  += OnLevelChanged;
    Skills.ActiveChanged += OnActiveChanged;
  }

  public void OnTreeExit() {
    Skills.Unlocked      -= OnUnlocked;
    Skills.LevelChanged  -= OnLevelChanged;
    Skills.ActiveChanged -= OnActiveChanged;
  }

  private void OnBuyPressed() {
    Service.UnlockOrUpgrade(SkillId);
    UpdateUi();
  }

  private void OnTogglePressed() {
    Service.Toggle(SkillId);
    UpdateUi();
  }

  private void OnUnlocked(string id)          { if (id == SkillId) UpdateUi(); }
  private void OnLevelChanged(string id,int _) { if (id == SkillId) UpdateUi(); }
  private void OnActiveChanged(string id,bool _){ if (id == SkillId) UpdateUi(); }

  private void UpdateUi() {
    var lvl = Skills.LevelOf(SkillId);
    var isUnlocked = Skills.IsUnlocked(SkillId);
    var isActive = Skills.IsActive(SkillId);

    Level.Text = isUnlocked ? $"Lv {lvl}" : "Locked";

    if (Catalog.TryGet(SkillId, out var def)) {
      var next = isUnlocked ? lvl + 1 : 1;
      var canUpgrade = next <= def.MaxLevel;
      var nextCost = def.BaseCost * next;

      Buy.Text = isUnlocked ? (canUpgrade ? $"Upgrade ({nextCost})" : "Max") : $"Buy ({def.BaseCost})";
      Buy.Disabled = !canUpgrade && isUnlocked;
      Toggle.Text = isActive ? "Deactivate" : "Activate";
      Toggle.Disabled = !isUnlocked;
    }
  }
}