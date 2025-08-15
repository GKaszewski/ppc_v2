using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.app;

namespace Mr.BrickAdventures.features.ui.skills;

[Meta(typeof(IAutoNode))]
public partial class SkillsPanel : Control
{
    public override void _Notification(int what) => this.Notify(what);

    [Node] private Container List { get; set; } = null!;
    [Export] public PackedScene RowScene { get; set; } = null!;

    [Dependency] private ISkillCatalog Catalog => this.DependOn<ISkillCatalog>();
    
    public void OnReady() {
        foreach (var id in Catalog.All.Keys.ToArray()) {
            var row = RowScene.Instantiate<SkillRow>();
            AddChild(row);
            row.Init(id);
            List.AddChild(row);
        }
    }
}