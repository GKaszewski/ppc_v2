using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Mr.BrickAdventures.data;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.game.services;

namespace Mr.BrickAdventures.common;

[Meta(typeof(IAutoNode))]
public partial class AppRoot : Node2D, IProvide<PlayerRepository>, IProvide<LevelRepository>, IProvide<SaveClient>, IProvide<SaveService>
{
    public override void _Notification(int what) => this.Notify(what);

    private readonly SaveClient _save = new("user://savegame.save", version: 2);
    private readonly PlayerRepository _players = new();
    private readonly LevelRepository _levels = new();
    private SaveService _saveService = null!;

    PlayerRepository IProvide<PlayerRepository>.Value() => _players;
    LevelRepository  IProvide<LevelRepository>.Value()  => _levels;
    SaveClient       IProvide<SaveClient>.Value()       => _save;
    SaveService      IProvide<SaveService>.Value()      => _saveService;
    

    public void OnReady()
    {
        _saveService = new SaveService(_players, _levels, _save);

        _saveService.TryLoad();

        this.Provide();
    }
}