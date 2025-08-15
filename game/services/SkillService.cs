using System.Linq;
using Mr.BrickAdventures.app;
using Mr.BrickAdventures.game.repositories;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.game.services;

public sealed class SkillService
{
    private readonly PlayerRepository _players;
    private readonly SkillsRepository _skills;
    private readonly ISkillCatalog _catalog;

    public SkillService(PlayerRepository players, SkillsRepository skills, ISkillCatalog catalog) {
        _players = players; _skills = skills; _catalog = catalog;
    }
    
    public bool CanAfford(string id) {
        if (!_catalog.TryGet(id, out var def)) return false;
        var nextLevel = _skills.IsUnlocked(id) ? _skills.LevelOf(id) + 1 : 1;
        if (nextLevel > def.MaxLevel) return false;
        var cost = NextLevelCost(def, nextLevel);
        return _players.Coins >= cost;
    }

    public bool UnlockOrUpgrade(string id) {
        if (!_catalog.TryGet(id, out var def)) return false;

        var nextLevel = _skills.IsUnlocked(id) ? _skills.LevelOf(id) + 1 : 1;
        if (nextLevel > def.MaxLevel) return false;

        var cost = NextLevelCost(def, nextLevel);
        if (_players.Coins < cost) return false;

        _players.RemoveCoins(cost);

        if (!_skills.IsUnlocked(id)) _skills.Unlock(id);
        _skills.SetLevel(id, nextLevel);

        if (nextLevel == 1 && def.Type == SkillType.Throw) ActivateExclusiveThrow(id);

        return true;
    }

    public void Toggle(string id) {
        if (!_catalog.TryGet(id, out var def)) return;
        if (!_skills.IsUnlocked(id)) return;

        var nowActive = _skills.IsActive(id);
        if (nowActive) {
            _skills.SetActive(id, false);
            return;
        }

        if (def.Type == SkillType.Throw) ActivateExclusiveThrow(id);
        else _skills.SetActive(id, true);
    }

    private void ActivateExclusiveThrow(string id) {
        foreach (var other in _skills.ActiveIds.ToArray()) {
            if (other == id) continue;
            if (_catalog.TryGet(other, out var od) && od.Type == SkillType.Throw) {
                _skills.SetActive(other, false);
            }
        }
        _skills.SetActive(id, true);
    }

    private static int NextLevelCost(SkillDef def, int nextLevel)
        => def.BaseCost * nextLevel;
}