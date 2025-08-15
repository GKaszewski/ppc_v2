using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.repositories;

namespace Mr.BrickAdventures.features.player.skills;

[Meta(typeof(IAutoNode))]
public partial class SkillEffectsAdapter : Node
{
    public override void _Notification(int what) => this.Notify(what);

    [Node("%SkillsAnchor")] private Node Anchor { get; set; } = null!;
    
    [Dependency] private SkillsRepository Skills => this.DependOn<SkillsRepository>();
    [Dependency] private ISkillCatalog Catalog => this.DependOn<ISkillCatalog>();

    private readonly Dictionary<string, Node> _instances = new();

    public void OnResolved()
    {
        Skills.ActiveChanged += OnActiveChanged;
        Skills.LevelChanged += OnLevelChanged;

        foreach (var id in Skills.ActiveIds) Apply(id, Skills.LevelOf(id));
    }

    public void OnExitTree()
    {
        Skills.ActiveChanged -= OnActiveChanged;
        Skills.LevelChanged -= OnLevelChanged;
    }
    
    private void OnActiveChanged(string id, bool active) {
        if (active) Apply(id, Skills.LevelOf(id)); else Remove(id);
    }

    private void OnLevelChanged(string id, int level) {
        if (_instances.TryGetValue(id, out var inst)) {
            if (inst is ISkillEffect cfg) { cfg.Configure(id, level); return; }
            // fallback: recreate
            Remove(id); Apply(id, level);
        }
    }
    
    private void Apply(string id, int level) {
        if (_instances.ContainsKey(id)) return;
        if (!Catalog.TryGet(id, out var def)) return;

        var packed = ResourceLoader.Load<PackedScene>(def.EffectScene);
        if (packed == null) {
            GD.PushWarning($"SkillEffectsAdapter: Cannot load scene {def.EffectScene} for {id}");
            return;
        }

        var inst = packed.Instantiate();
        InjectConfig(inst, def);
        (Anchor ?? Owner ?? this).AddChild(inst);
        if (inst is ISkillEffect cfg) cfg.Configure(id, level);
        _instances[id] = inst;
    }

    private void Remove(string id) {
        if (!_instances.TryGetValue(id, out var node)) return;
        if (IsInstanceValid(node)) node.QueueFree();
        _instances.Remove(id);
    }

    private void InjectConfig(Node instance, SkillDef def) {
        if (def.Config == null) return;

        foreach (var kv in def.Config) {
            var key = kv.Key;
            var raw = kv.Value;

            Variant value = raw;

            // Resolve NodePath → Node (prefer inside instance, fallback to Owner tree)
            if (raw.VariantType == Variant.Type.NodePath) {
                var np = (NodePath)raw;
                if      (instance.HasNode(np)) value = instance.GetNode(np);
                else if ((Owner ?? this).HasNode(np)) value = (Owner ?? this).GetNode(np);
                else continue;
            }
            // Load Godot resource/scene if a string "res://...":
            else if (raw.VariantType == Variant.Type.String && ((string)raw).StartsWith("res://")) {
                var path = (string)raw;
                var res = ResourceLoader.Load(path);
                if (res != null) value = res;
            }

            // Set via property if exists (Godot C# will reject unknown properties silently)
            instance.Set(key, value);
        }
    }
}