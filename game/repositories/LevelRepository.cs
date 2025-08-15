using System;
using System.Collections.Generic;
using System.Linq;

namespace Mr.BrickAdventures.game.repositories;

public sealed class LevelRepository
{
    public int Current { get; private set; } = 0;
    public HashSet<int> Unlocked { get; } = new() { 0 };
    public HashSet<int> Completed { get; } = new();

    public event Action<int>? CurrentChanged;

    public void SetCurrent(int idx) { Current = idx; CurrentChanged?.Invoke(Current); }
    public void Unlock(int idx) => Unlocked.Add(idx);
    public void Complete(int idx) { Completed.Add(idx); Unlock(idx + 1); }

    public LevelState Export() => new() {
        Current = Current, Unlocked = [..Unlocked], Completed = [..Completed]
    };
    public void Load(LevelState s) {
        Current = s.Current;
        Unlocked.Clear(); foreach (var i in s.Unlocked) Unlocked.Add(i);
        Completed.Clear(); foreach (var i in s.Completed) Completed.Add(i);
        CurrentChanged?.Invoke(Current);
    }
}

public record LevelState {
    public int Current;
    public int[] Unlocked = [];
    public int[] Completed = [];
}