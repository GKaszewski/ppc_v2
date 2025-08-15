using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.data;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.common;

[Meta(typeof(IAutoNode))]
public partial class AppRoot : Node2D,
    IProvide<PlayerRepository>,
    IProvide<LevelRepository>,
    IProvide<SaveClient>,
    IProvide<SaveService>,
    IProvide<LevelService>,
    IProvide<IGameScenes>,
    IProvide<ILevelCatalog>,
    IProvide<ISkillCatalog>,
    IGameScenes
{
    public override void _Notification(int what) => this.Notify(what);

    [Export] private Array<PackedScene> _levels = [];
    [Export] private PackedScene _mainMenu = null!;

    private readonly SaveClient _save = new("user://savegame.save", version: 3);
    private readonly PlayerRepository _players = new();
    private readonly LevelRepository _levelsRepo = new();
    private readonly SkillsRepository _skills = new();
    private SaveService _saveService = null!;
    private LevelService _levelService = null!;
    private ILevelCatalog _levelCatalog = null!;
    private ISkillCatalog _skillCatalog = null!;
    private SkillService _skillService = null!;

    PlayerRepository IProvide<PlayerRepository>.Value() => _players;
    LevelRepository IProvide<LevelRepository>.Value() => _levelsRepo;
    SaveClient IProvide<SaveClient>.Value() => _save;
    SaveService IProvide<SaveService>.Value() => _saveService;
    LevelService IProvide<LevelService>.Value() => _levelService;
    ILevelCatalog IProvide<ILevelCatalog>.Value() => _levelCatalog;
    ISkillCatalog IProvide<ISkillCatalog>.Value() => _skillCatalog;
    IGameScenes IProvide<IGameScenes>.Value() => this;


    public void OnReady()
    {
        _skillCatalog = new AppSkillCatalog();
        _levelCatalog = new ExportedLevelCatalog(_levels, _mainMenu);

        _saveService = new SaveService(_players, _levelsRepo, _skills, _save);
        _levelService = new LevelService(_levelsRepo);
        _skillService = new SkillService(_players, _skills, _skillCatalog);

        _saveService.TryLoad();
        this.Provide();
    }

    public void Load(PackedScene scene) => GetTree().ChangeSceneToPacked(scene);
    public void Restart() => GetTree().ReloadCurrentScene();
    public void ReturnToMain(PackedScene mainMenu) => GetTree().ChangeSceneToPacked(mainMenu);
}