using System;
using Godot;
using Steamworks;
using Steamworks.Data;

namespace Mr.BrickAdventures.Autoloads;

public partial class SteamManager : Node
{
    private const uint AppId = 3575090;
    
    public static string PlayerName { get; private set; } = "Player";
    public static bool IsSteamInitialized { get; private set; } = false;

    public override void _Ready()
    {
        try
        {
            SteamClient.Init(AppId);
            IsSteamInitialized = true;

            PlayerName = SteamClient.Name;

            GD.Print($"Steam initialized successfully for user: {PlayerName}");
        }
        catch (Exception e)
        {
            GD.PushError("Failed to initialize Steamworks. Is Steam running?");
            GD.PushError(e.Message);
            IsSteamInitialized = false;
        }
    }

    public override void _Process(double delta)
    {
        if (IsSteamInitialized) SteamClient.RunCallbacks();
    }
    
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            if (IsSteamInitialized)
            {
                SteamClient.Shutdown();
            }
            GetTree().Quit();
        }
    }

    public static void UnlockAchievement(string achievementId)
    {
        if (!IsSteamInitialized)
        {
            GD.Print($"Steam not initialized. Cannot unlock achievement '{achievementId}'.");
            return;
        }

        var ach = new Achievement(achievementId);

        if (ach.State)
        {
            GD.Print($"Achievement '{achievementId}' is already unlocked.");
            return;
        }

        if (ach.Trigger())
        {
            SteamUserStats.StoreStats();
            GD.Print($"Successfully triggered achievement: {ach.Name} ({ach.Identifier})");
        }
        else
        {
            GD.PrintErr($"Failed to trigger achievement: {achievementId}");
        }
    }
}