using Mr.BrickAdventures.data;
using Mr.BrickAdventures.game.repositories;

namespace Mr.BrickAdventures.game.services;

public sealed class SaveService
{
    private readonly PlayerRepository _players;
    private readonly LevelRepository _levels;
    private readonly SkillsRepository _skills;
    private readonly SaveClient _save;
    
    public SaveService(PlayerRepository players, LevelRepository levels, SkillsRepository skills, SaveClient save) {
        _players = players; _levels = levels; _skills = skills; _save = save;
    }

    public bool TryLoad() {
        if (!_save.TryLoad(out var s)) return false;
        _players.Load(s.PlayerState);
        _levels.Load(s.LevelState);
        _skills.Load(s.SkillsState);
        return true;
    }

    public void Save() => _save.Save(new SaveSnapshot {
        PlayerState = _players.Export(),
        LevelState  = _levels.Export(),
        SkillsState = _skills.Export()
    });

    public bool Exists() => _save.Exists();

    public void NewGame() {
        _players.Load(new PlayerState { Coins = 0, Lives = 3 });
        _levels.Load(new LevelState { Current = 0, Unlocked = [0], Completed = [] });
        _skills.Load(SkillsRepository.NewGame());
        Save();
    }
}   