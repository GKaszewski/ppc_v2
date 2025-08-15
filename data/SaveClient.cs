using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Mr.BrickAdventures.game.repositories;
using GDDict = Godot.Collections.Dictionary;
using GDArrayString = Godot.Collections.Array<string>;
using GDArrayInt = Godot.Collections.Array<int>;
using CSLevels = System.Collections.Generic.Dictionary<string, int>;

namespace Mr.BrickAdventures.data;

public sealed class SaveClient
{
    private readonly string _path;
    private readonly int _version;

    public SaveClient(string path, int version)
    {
        _path = path;
        _version = version;
    }

    public bool Exists() => FileAccess.FileExists(_path);

    public bool TryLoad(out SaveSnapshot snapshot)
    {
        snapshot = null!;
        if (!Exists()) return false;

        using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Read);
        var dict = (Dictionary)f.GetVar();

        if ((int)dict.GetValueOrDefault("version", -1) != _version) return false;

        snapshot = new SaveSnapshot
        {
            PlayerState = ToPlayer((Dictionary)dict["player_state"]),
            LevelState = ToLevel((Dictionary)dict["level_state"]),
            SkillsState = ToSkills((Dictionary)dict["skills_state"])
        };
        return true;
    }

    // Strict load: requires version + player_state + level_state.
    // If anything is off, delete the file and act as "no save".
    public bool TryLoadStrict(out SaveSnapshot snapshot)
    {
        snapshot = null!;

        if (!Exists()) return false;

        try
        {
            using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Read);
            if (f == null)
            {
                Delete();
                return false;
            }

            var dictionary = (Dictionary)f.GetVar();


            if (!dictionary.TryGetValue("version", out var v) || (int)v != _version)
            {
                Delete();
                return false;
            }

            if (!dictionary.TryGetValue("player_state", out var pObj) || (Dictionary)pObj is not { } p)
            {
                Delete();
                return false;
            }

            if (!dictionary.TryGetValue("level_state", out var lObj) || (Dictionary)lObj is not { } l)
            {
                Delete();
                return false;
            }

            if (!dictionary.TryGetValue("skills_state", out var sObj) || (Dictionary)sObj is not { } s)
            {
                Delete();
                return false;
            }

            snapshot = new SaveSnapshot
            {
                PlayerState = ToPlayer(p),
                LevelState = ToLevel(l),
                SkillsState = ToSkills(s)
            };
            return true;
        }
        catch (Exception e)
        {
            GD.PushWarning($"SaveClient: load failed — deleting bad save. {e.GetType().Name}: {e.Message}");
            Delete();
            return false;
        }
    }

    public void Save(SaveSnapshot s)
    {
        using var f = FileAccess.Open(_path, FileAccess.ModeFlags.Write);
        var dict = new Dictionary
        {
            { "version", _version },
            { "player_state", FromPlayer(s.PlayerState) },
            { "level_state", FromLevel(s.LevelState) },
            { "skills_state", FromSkills(s.SkillsState) }
        };
        f.StoreVar(dict);
    }

    public void Delete()
    {
        if (!Exists()) return;
        var abs = ProjectSettings.GlobalizePath(_path);
        var ok = DirAccess.RemoveAbsolute(abs);
        if (ok != Error.Ok) GD.PushWarning($"SaveClient: failed to delete {_path}: {ok}");
    }

    private static GDDict FromPlayer(PlayerState s) => new() { { "coins", s.Coins }, { "lives", s.Lives } };

    private static PlayerState ToPlayer(GDDict d) => new()
    {
        Coins = d.TryGetValue("coins", out var c) ? (int)c : 0,
        Lives = d.TryGetValue("lives", out var l) ? (int)l : 3
    };

    private static GDDict FromLevel(LevelState s) => new()
    {
        { "current", s.Current },
        { "unlocked", new GDArrayInt(s.Unlocked) },
        { "completed", new GDArrayInt(s.Completed) },
    };

    private static LevelState ToLevel(GDDict d) => new()
    {
        Current = d.TryGetValue("current", out var cur) ? (int)cur : 0,
        Unlocked = d.TryGetValue("unlocked", out var ul) && (GDArrayInt)ul is { } a1 ? a1.ToArray() : [0],
        Completed = d.TryGetValue("completed", out var cl) && (GDArrayInt)cl is { } a2 ? a2.ToArray() : [],
    };

    private static GDDict FromSkills(SkillsState s) => new()
    {
        { "unlocked", new GDArrayString(s.Unlocked) },
        { "active", new GDArrayString(s.Active) },
        { "levels", ToGodotDict(s.Levels) }
    };

    private static SkillsState ToSkills(GDDict d) => new()
    {
        Unlocked = d.TryGetValue("unlocked", out var ul) && (GDArrayString)ul is { } arrU
            ? arrU.ToArray()
            : [],
        Active = d.TryGetValue("active", out var ac) && (GDArrayString)ac is { } arrA
            ? arrA.ToArray()
            : [],
        Levels = d.TryGetValue("levels", out var lv) && (GDDict)lv is { } gdLevels
            ? ToCsStringIntDict(gdLevels)
            : new CSLevels()
    };

    private static GDDict ToGodotDict(CSLevels src)
    {
        var gd = new GDDict();
        foreach (var kv in src) gd[kv.Key] = kv.Value;
        return gd;
    }

    private static CSLevels ToCsStringIntDict(GDDict src)
    {
        var dst = new CSLevels();
        foreach (var e in src)
        {
            var key = (string)e.Key is { } s ? s : e.Key.ToString() ?? "";
            var value = (int)e.Value is {} i ? i : 0;
            dst[key] = value;
        }

        return dst;
    }
}

public record SaveSnapshot
{
    public required PlayerState PlayerState { get; init; }
    public required LevelState LevelState { get; init; }
    public required SkillsState SkillsState { get; init; }
}