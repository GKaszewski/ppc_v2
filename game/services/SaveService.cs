using Mr.BrickAdventures.data;
using Mr.BrickAdventures.game.repositories;

namespace Mr.BrickAdventures.game.services;

public sealed class SaveService
{
    private readonly PlayerRepository _players;
    private readonly LevelRepository _levels;
    private readonly SaveClient _save;
    
    public SaveService(PlayerRepository players, LevelRepository levels, SaveClient save) {
        _players = players; _levels = levels; _save = save;
    }
    
    
    public bool TryLoad() {
        if (!_save.TryLoad(out var p, out var l)) return false;
        _players.Load(p);
        _levels.Load(l);
        return true;
    }

    public void Save() => _save.Save(_players.Export(), _levels.Export());
    public bool Exists() => _save.Exists();

    public void NewGame() {
        _players.Load(new PlayerState { Coins = 0, Lives = 3 });
        _levels.Load(new LevelState { Current = 0, Unlocked = [0], Completed = [] });
        Save();
    }
}   