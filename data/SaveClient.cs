using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.game.repositories;

namespace Mr.BrickAdventures.data;

public sealed class SaveClient
{
    private readonly string _path;
    private readonly int _version;

    public SaveClient(string path, int version) { _path = path; _version = version; }

    public bool Exists() => FileAccess.FileExists(_path);

    public bool TryLoad(out PlayerState player, out LevelState level) {
        player = null!; level = null!;
        if (!Exists()) return false;

        using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Read);
        var dict = (Dictionary)f.GetVar();

        if ((int)dict.GetValueOrDefault("version", -1) != _version) return false;

        player = ToPlayer((Dictionary)dict["player"]);
        level  = ToLevel((Dictionary)dict["level"]);
        return true;
    }
    
    // Strict load: requires version + player_state + level_state.
    // If anything is off, delete the file and act as "no save".
    public bool TryLoadStrict(out PlayerState player, out LevelState level) {
        player = default!;
        level  = default!;

        if (!Exists()) return false;

        try {
            using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Read);
            if (f == null) { Delete(); return false; }

            var dictionary = (Dictionary)f.GetVar();
            

            if (!dictionary.TryGetValue("version", out var v) || (int)v != _version) { Delete(); return false; }

            if (!dictionary.TryGetValue("player_state", out var pObj) || (Dictionary)pObj is not { } p) { Delete(); return false; }
            if (!dictionary.TryGetValue("level_state",  out var lObj) || (Dictionary)lObj is not { } l) { Delete(); return false; }

            player = ToPlayer(p);
            level  = ToLevel(l);
            return true;
        }
        catch (Exception e) {
            GD.PushWarning($"SaveClient: load failed — deleting bad save. {e.GetType().Name}: {e.Message}");
            Delete();
            return false;
        }
    }

    public void Save(PlayerState player, LevelState level) {
        using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Write);
        var dict = new Dictionary {
            { "version", _version },
            { "player", FromPlayer(player) },
            { "level",  FromLevel(level) }
        };
        f.StoreVar(dict);
    }
    
    public void Delete() {
        if (!Exists()) return;
        var abs = ProjectSettings.GlobalizePath(_path);
        var ok = DirAccess.RemoveAbsolute(abs);
        if (ok != Error.Ok) GD.PushWarning($"SaveClient: failed to delete {_path}: {ok}");
    }

    private static Dictionary FromPlayer(PlayerState s) => new() { { "coins", s.Coins }, { "lives", s.Lives } };
    private static PlayerState ToPlayer(Dictionary d) => new() {
        Coins = d.TryGetValue("coins", out var c) ? (int)c : 0,
        Lives = d.TryGetValue("lives", out var l) ? (int)l : 3
    };

    private static Dictionary FromLevel(LevelState s) => new() {
        { "current",   s.Current },
        { "unlocked",  new Array<int>(s.Unlocked) },
        { "completed", new Array<int>(s.Completed) },
    };

    private static LevelState ToLevel(Dictionary d) => new() {
        Current   = d.TryGetValue("current", out var cur) ? (int)cur : 0,
        Unlocked  = d.TryGetValue("unlocked", out var ul) && (Array<int>)ul is { } a1 ? a1.ToArray() : [0],
        Completed = d.TryGetValue("completed", out var cl) && (Array<int>)cl is { } a2 ? a2.ToArray() : [],
    };
}

public record SaveSnapshot {
    public required PlayerState PlayerState { get; init; }
    public required LevelState  LevelState  { get; init; }
}