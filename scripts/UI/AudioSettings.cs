using Godot;
using Mr.BrickAdventures;
using Mr.BrickAdventures.Autoloads;

namespace Mr.BrickAdventures.scripts.UI;

public partial class AudioSettings : Control
{
    [Export] public Slider MasterVolumeSlider { get; set; }
    [Export] public Slider MusicVolumeSlider { get; set; }
    [Export] public Slider SfxVolumeSlider { get; set; }
    [Export] public Control AudioSettingsControl { get; set; }
    [Export] public float MuteThreshold { get; set; } = -20f;

    private UIManager _uiManager;
    private ConfigFileHandler _configFileHandler;

    public override void _Ready()
    {
        _uiManager = GetNode<UIManager>(Constants.UIManagerPath);
        _configFileHandler = GetNode<ConfigFileHandler>(Constants.ConfigFileHandlerPath);
        Initialize();
        MasterVolumeSlider.ValueChanged += OnMasterVolumeChanged;
        MusicVolumeSlider.ValueChanged += OnMusicVolumeChanged;
        SfxVolumeSlider.ValueChanged += OnSfxVolumeChanged;

        LoadSettings();
    }

    public override void _ExitTree()
    {
        SaveSettings();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionReleased("ui_cancel")) return;
        if (!_uiManager.IsScreenOnTop(AudioSettingsControl)) return;

        SaveSettings();
        _uiManager.PopScreen();
    }

    private void OnSfxVolumeChanged(double value)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("sfx"), (float)value);
        HandleMute(AudioServer.GetBusIndex("sfx"), (float)value);
    }

    private void OnMusicVolumeChanged(double value)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), (float)value);
        HandleMute(AudioServer.GetBusIndex("music"), (float)value);
    }

    private void OnMasterVolumeChanged(double value)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), (float)value);
        HandleMute(AudioServer.GetBusIndex("Master"), (float)value);
    }

    private void Initialize()
    {
        var volumeDb = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));
        MasterVolumeSlider.Value = volumeDb;
        MasterVolumeSlider.MinValue = MuteThreshold;
        MasterVolumeSlider.MaxValue = 0f;

        var musicVolumeDb = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("music"));
        MusicVolumeSlider.Value = musicVolumeDb;
        MusicVolumeSlider.MinValue = MuteThreshold;
        MusicVolumeSlider.MaxValue = 0f;

        var sfxVolumeDb = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("sfx"));
        SfxVolumeSlider.Value = sfxVolumeDb;
        SfxVolumeSlider.MinValue = MuteThreshold;
        SfxVolumeSlider.MaxValue = 0f;
    }

    private void HandleMute(int busIndex, float value)
    {
        AudioServer.SetBusMute(busIndex, value <= MuteThreshold);
    }

    private void SaveSettings()
    {
        var settingsConfig = _configFileHandler.SettingsConfig;
        settingsConfig.SetValue("audio_settings", "master_volume", MasterVolumeSlider.Value);
        settingsConfig.SetValue("audio_settings", "music_volume", MusicVolumeSlider.Value);
        settingsConfig.SetValue("audio_settings", "sfx_volume", SfxVolumeSlider.Value);
        settingsConfig.SetValue("audio_settings", "mute_threshold", MuteThreshold);
        settingsConfig.Save(ConfigFileHandler.SettingsPath);
    }

    private void LoadSettings()
    {
        var settingsConfig = _configFileHandler.SettingsConfig;
        if (!settingsConfig.HasSection("audio_settings")) return;

        var masterVolume = (float)settingsConfig.GetValue("audio_settings", "master_volume", MasterVolumeSlider.Value);
        var musicVolume = (float)settingsConfig.GetValue("audio_settings", "music_volume", MusicVolumeSlider.Value);
        var sfxVolume = (float)settingsConfig.GetValue("audio_settings", "sfx_volume", SfxVolumeSlider.Value);
        var muteThreshold = (float)settingsConfig.GetValue("audio_settings", "mute_threshold", MuteThreshold);

        MasterVolumeSlider.Value = masterVolume;
        MusicVolumeSlider.Value = musicVolume;
        SfxVolumeSlider.Value = sfxVolume;
        MuteThreshold = muteThreshold;
    }
}