using System;
using System.Collections.Generic;
using System.Linq;

namespace Mr.BrickAdventures.game.repositories;

public sealed class SkillsRepository
{
    private readonly HashSet<string> _unlocked = [];
    private readonly HashSet<string> _active = [];
    private readonly Dictionary<string, int> _levels = [];
    
    public event Action<string>? Unlocked;
    public event Action<string, bool>? ActiveChanged;
    public event Action<string, int>? LevelChanged;

    public IEnumerable<string> UnlockedIds => _unlocked;
    public IEnumerable<string> ActiveIds => _active;
    
    public bool IsUnlocked(string id) => _unlocked.Contains(id);
    public bool IsActive(string id) => _active.Contains(id);
    public int LevelOf(string id) => _levels.TryGetValue(id, out var level) ? level : 0;

    public void Unlock(string id)
    {
        if (_unlocked.Add(id))
        {
            if (!_levels.ContainsKey(id)) _levels[id] = 1;
            Unlocked?.Invoke(id);
        }
    }

    public void SetLevel(string id, int level)
    {
        level = Math.Max(1, level);
        var changed = !_levels.TryGetValue(id, out var current) || current != level;
        _levels[id] = level;
        if (changed) LevelChanged?.Invoke(id, level);
    }

    public void SetActive(string id, bool active)
    {
        var changed = active ? _active.Add(id) : _active.Remove(id);
        if (changed) ActiveChanged?.Invoke(id, active);
    }

    public SkillsState Export() => new()
    {
        Unlocked = _unlocked.ToArray(),
        Active   = _active.ToArray(),
        Levels   = _levels.ToDictionary(kv => kv.Key, kv => kv.Value)
    };

    public void Load(SkillsState state)
    {
        _unlocked.Clear(); foreach (var id in state.Unlocked) _unlocked.Add(id);
        _active.Clear(); foreach (var id in state.Active) _active.Add(id);
        _levels.Clear(); foreach (var kv in state.Levels) _levels[kv.Key] = kv.Value;
        
        foreach(var u in _unlocked) Unlocked?.Invoke(u);
        foreach(var a in _active) ActiveChanged?.Invoke(a, true);
        foreach(var kv in _levels) LevelChanged?.Invoke(kv.Key, kv.Value);
    }

    public static SkillsState NewGame() => new()
    {
        Unlocked = [],
        Active   = [],
        Levels   = new Dictionary<string, int>()
    };
}

public record SkillsState {
    public string[] Unlocked = [];
    public string[] Active   = [];
    public Dictionary<string,int> Levels = new();
}