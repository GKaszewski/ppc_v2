using System.Collections.Generic;
using System.Text.Json;
using Godot;
using Mr.BrickAdventures.scripts.Resources;
using Mr.BrickAdventures.scripts.State;

namespace Mr.BrickAdventures.Autoloads;

/// <summary>
/// Save system that serializes state to JSON using DTOs.
/// </summary>
public partial class SaveSystem : Node
{
    [Export] public string SavePath { get; set; } = "user://savegame.json";
    [Export] public int Version { get; set; } = 2;

    public static SaveSystem Instance { get; private set; }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this) Instance = null;
    }

    public void SaveGame()
    {
        var store = GameStateStore.Instance;
        if (store == null)
        {
            GD.PrintErr("SaveSystem: GameStateStore not available.");
            return;
        }

        // Convert to DTO (only serializable data)
        var saveData = new SaveDataDto
        {
            Version = Version,
            Coins = store.Player.Coins,
            Lives = store.Player.Lives,
            CurrentLevel = store.Session.CurrentLevel,
            CompletedLevels = [.. store.Player.CompletedLevels],
            UnlockedLevels = new List<int>(store.Player.UnlockedLevels),
            UnlockedSkillNames = GetSkillNames(store.Player.UnlockedSkills),
            UnlockedAchievements = new List<string>(store.Player.UnlockedAchievements),
            Statistics = new Dictionary<string, int>(store.Player.Statistics)
        };

        try
        {
            var json = JsonSerializer.Serialize(saveData, JsonOptions);
            using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
            file.StoreString(json);
            GD.Print("Game saved to: ", SavePath);
            EventBus.EmitGameSaved();
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"SaveSystem: Failed to save game: {e.Message}");
        }
    }

    public bool LoadGame()
    {
        if (!FileAccess.FileExists(SavePath))
        {
            GD.Print("SaveSystem: No save file found.");
            return false;
        }

        try
        {
            using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
            var json = file.GetAsText();
            var saveData = JsonSerializer.Deserialize<SaveDataDto>(json, JsonOptions);

            if (saveData == null)
            {
                GD.PrintErr("SaveSystem: Failed to deserialize save data.");
                return false;
            }

            if (saveData.Version != Version)
            {
                GD.PrintErr($"SaveSystem: Version mismatch. Expected {Version}, found {saveData.Version}");
                return false;
            }

            var store = GameStateStore.Instance;
            if (store == null)
            {
                GD.PrintErr("SaveSystem: GameStateStore not available.");
                return false;
            }

            // Apply loaded state
            store.Player.Coins = saveData.Coins;
            store.Player.Lives = saveData.Lives;
            store.Session.CurrentLevel = saveData.CurrentLevel;

            store.Player.CompletedLevels = saveData.CompletedLevels ?? new List<int>();
            store.Player.UnlockedLevels = saveData.UnlockedLevels ?? new List<int> { 0 };
            store.Player.UnlockedAchievements = saveData.UnlockedAchievements ?? new List<string>();
            store.Player.Statistics = saveData.Statistics ?? new Dictionary<string, int>();

            // Reload skills by name from SkillManager
            store.Player.UnlockedSkills = LoadSkillsByName(saveData.UnlockedSkillNames);

            GD.Print("Game loaded from: ", SavePath);
            return true;
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"SaveSystem: Failed to load game: {e.Message}");
            return false;
        }
    }

    private static List<string> GetSkillNames(List<SkillData> skills)
    {
        var names = new List<string>();
        foreach (var skill in skills)
        {
            if (skill != null)
                names.Add(skill.Name);
        }
        return names;
    }

    private List<SkillData> LoadSkillsByName(List<string> skillNames)
    {
        var skills = new List<SkillData>();
        if (skillNames == null) return skills;

        var skillManager = GetNodeOrNull<SkillManager>(Constants.SkillManagerPath);
        if (skillManager == null)
        {
            GD.PrintErr("SaveSystem: SkillManager not available to resolve skill names.");
            return skills;
        }

        foreach (var name in skillNames)
        {
            var skill = skillManager.GetSkillByName(name);
            if (skill != null)
            {
                skills.Add(skill);
            }
            else
            {
                GD.PrintErr($"SaveSystem: Skill '{name}' not found in SkillManager.");
            }
        }
        return skills;
    }

    public bool CheckSaveExists() => FileAccess.FileExists(SavePath);

    public void DeleteSave()
    {
        if (FileAccess.FileExists(SavePath))
        {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SavePath));
            GD.Print("Save file deleted.");
        }
    }
}

/// <summary>
/// Serializable DTO for save data - no Godot types.
/// </summary>
public class SaveDataDto
{
    public int Version { get; set; }
    public int Coins { get; set; }
    public int Lives { get; set; }
    public int CurrentLevel { get; set; }
    public List<int> CompletedLevels { get; set; }
    public List<int> UnlockedLevels { get; set; }
    public List<string> UnlockedSkillNames { get; set; }
    public List<string> UnlockedAchievements { get; set; }
    public Dictionary<string, int> Statistics { get; set; }
}