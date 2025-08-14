using Godot;

namespace Mr.BrickAdventures.Autoloads;

public partial class ConfigFileHandler : Node
{
    private ConfigFile _settingsConfig = new();
    public const string SettingsPath = "user://settings.ini";
    
    public ConfigFile SettingsConfig => _settingsConfig;

    public override void _Ready()
    {
        if (!FileAccess.FileExists(SettingsPath))
        {
            var err = _settingsConfig.Save(SettingsPath);
            if (err != Error.Ok)
                GD.PushError($"Failed to create settings file at {SettingsPath}: {err}");
        }
        else
        {
            var err = _settingsConfig.Load(SettingsPath);
            if (err != Error.Ok)
                GD.PushError($"Failed to load settings file at {SettingsPath}: {err}");
        }
    }
}