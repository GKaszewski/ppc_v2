using Godot;
using Mr.BrickAdventures.scripts.interfaces;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.scripts.components;

[GlobalClass]
public partial class XRayVisionSkillComponent : SkillComponentBase
{
    [Export(PropertyHint.Layers2DRender)] public uint SecretLayer { get; set; }
    [Export] public float Duration { get; set; } = 5.0f;

    private Camera2D _camera;
    private Viewport _viewport;
    private uint _originalVisibilityLayer;
    private Timer _timer;

    public override void Initialize(Node owner, SkillData data)
    {
        base.Initialize(owner, data);
        _viewport = GetViewport();
        _camera = GetViewport().GetCamera2D();
        _timer = new Timer { OneShot = true };
        AddChild(_timer);
        _timer.Timeout += Deactivate;
    }

    public override void Activate()
    {
        if (_camera == null) return;

        _originalVisibilityLayer = _camera.VisibilityLayer;
        _camera.VisibilityLayer |= SecretLayer;
        _timer.Start(Duration);
    }

    public override void Deactivate()
    {
        if (_camera != null)
        {
            _camera.VisibilityLayer = _originalVisibilityLayer;
        }
    }

    public override void ApplyUpgrade(SkillUpgrade upgrade)
    {
        if (upgrade.Properties.TryGetValue("duration", out var newDuration))
        {
            Duration = (float)newDuration;
        }
    }
}