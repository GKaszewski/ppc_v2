using Mr.BrickAdventures.game.repositories;

namespace Mr.BrickAdventures.game.services;

public sealed class LevelService
{
    private readonly LevelRepository _levels;
    public LevelService(LevelRepository levels) => _levels = levels;
    
    public int CompleteAndAdvance() {
        var cur = _levels.Current;
        _levels.Complete(cur);
        var next = cur + 1;
        _levels.SetCurrent(next);
        return next;
    }
    
    public void StartNew() {
        _levels.Load(new LevelState { Current = 0, Unlocked = new [] { 0 }, Completed = [] });
    }
}