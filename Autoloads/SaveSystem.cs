using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.Autoloads;

public partial class SaveSystem : Node
{
    [Export] public string SavePath { get; set; } = "user://savegame.save";
    [Export] public int Version { get; set; } = 1;

    //private GM _gm;

    public override void _Ready()
    {
        //_gm = GetNode<GM>("/root/GameManager");
    }

    public void SaveGame()
    {
        //TODO: Implement saving logic
    }

    public bool LoadGame()
    {
        //TODO: Implement loading logic
        
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

        return true;
    }
    
    public bool CheckSaveExists() => FileAccess.FileExists(SavePath);
}