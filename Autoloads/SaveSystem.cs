using Godot;
using Godot.Collections;
using Mr.BrickAdventures.scripts.Resources;

namespace Mr.BrickAdventures.Autoloads;

public partial class SaveSystem : Node
{
    [Export] public string SavePath { get; set; } = "user://savegame.save";
    [Export] public int Version { get; set; } = 1;

    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public void SaveGame()
    {
        var saveData = new Dictionary
        {
            { "player_state", _gameManager.PlayerState},
            { "version", Version}
        };

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        file.StoreVar(saveData);
        GD.Print("Game state saved to: ", SavePath);
    }

    public bool LoadGame()
    {
        if (!FileAccess.FileExists(SavePath))
            return false;

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        var saveDataObj = (Dictionary)file.GetVar();

        if (saveDataObj.ContainsKey("version") && (int)saveDataObj["version"] != Version)
        {
            GD.Print($"Save file version mismatch. Expected: {Version}, Found: {saveDataObj["version"]}");
            return false;
        }

        GD.Print("Game state loaded from: ", SavePath);
        GD.Print("Player state: ", saveDataObj["player_state"]);
        _gameManager.PlayerState = (Dictionary)saveDataObj["player_state"];
        
        var skills = new Array<SkillData>();
        foreach (var skill in (Array<SkillData>)_gameManager.PlayerState["unlocked_skills"])
        {
            skills.Add(skill);
        }
        
        _gameManager.UnlockSkills(skills);
        return true;
    }
    
    public bool CheckSaveExists() => FileAccess.FileExists(SavePath);
}